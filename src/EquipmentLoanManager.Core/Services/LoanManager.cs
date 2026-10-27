using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

/// <summary>Runs all borrowing, returning and waiting list actions.</summary>
public sealed class LoanManager
{
    private const string LoanIdPrefix = "LN";

    private readonly IIdGenerator _idGenerator;
    private readonly IBorrowingPolicy _policy;

    /// <summary>Creates the manager from its services.</summary>
    public LoanManager(
        IInventory inventory,
        IBorrowerRegistry borrowers,
        ILoanLedger loans,
        IWaitingListRegistry waitingLists,
        IBorrowingPolicy policy,
        IIdGenerator idGenerator)
    {
        Inventory = inventory;
        Borrowers = borrowers;
        Loans = loans;
        WaitingLists = waitingLists;
        _policy = policy;
        _idGenerator = idGenerator;
    }

    /// <summary>The equipment inventory.</summary>
    public IInventory Inventory { get; }

    /// <summary>The borrower registry.</summary>
    public IBorrowerRegistry Borrowers { get; }

    /// <summary>The loan ledger.</summary>
    public ILoanLedger Loans { get; }

    /// <summary>The waiting lists.</summary>
    public IWaitingListRegistry WaitingLists { get; }

    /// <summary>Creates a manager with the given stock.</summary>
    public static LoanManager Create(IReadOnlyDictionary<EquipmentCategory, int> quantities)
    {
        var idGenerator = new SequentialIdGenerator();
        var borrowers = new BorrowerRegistry(idGenerator);
        var loans = new LoanLedger();

        return new LoanManager(
            new Inventory(quantities),
            borrowers,
            loans,
            new WaitingListRegistry(),
            new BorrowingPolicy(borrowers, loans),
            idGenerator);
    }

    /// <summary>Adds a borrower.</summary>
    public OperationResult<Borrower> AddBorrower(string? name, string? id = null) => Borrowers.Add(name, id);

    /// <summary>Removes a borrower with no active loans and cancels their waiting requests.</summary>
    public OperationResult RemoveBorrower(string? borrowerId)
    {
        var borrower = Borrowers.Find(borrowerId);
        if (borrower is null)
            return OperationResult.Failure("Select a borrower to remove.");

        var activeCount = Loans.ActiveFor(borrower.Id).Count;
        if (activeCount > 0)
            return OperationResult.Failure(
                $"{borrower.Name} still has {activeCount} active loan(s). Return them before removing.");

        var cancelled = WaitingLists.RemoveEverywhere(borrower.Id);
        Borrowers.Remove(borrower.Id);

        var suffix = cancelled > 0 ? $" {cancelled} waiting-list request(s) cancelled." : string.Empty;
        return OperationResult.Success($"Removed {borrower.DisplayName}.{suffix}");
    }

    /// <summary>Lends an available item to an eligible borrower.</summary>
    public OperationResult<Loan> Borrow(string? borrowerId, EquipmentCategory category)
    {
        var label = EquipmentCatalog.Label(category);

        if (Inventory.ConfiguredCount(category) == 0)
            return OperationResult<Loan>.Failure($"No {label} items were configured for this session.");

        var eligibility = _policy.CheckEligibility(borrowerId, category);
        if (!eligibility.Succeeded)
            return OperationResult<Loan>.Failure(eligibility.Message);

        var waiting = WaitingLists.GetQueue(category).Count;
        if (waiting > 0)
            return OperationResult<Loan>.Failure(
                $"{waiting} borrower(s) are already waiting for a {label}. Join the waiting list instead.");

        var item = Inventory.FindAvailable(category);
        if (item is null)
            return OperationResult<Loan>.Failure($"All {label} items are on loan. Join the waiting list instead.");

        return Issue(Borrowers.Find(borrowerId)!, item);
    }

    /// <summary>Returns a loaned item.</summary>
    public OperationResult<Loan> Return(string? loanId)
    {
        var loan = Loans.Find(loanId);
        if (loan is null)
            return OperationResult<Loan>.Failure("Select an active loan to return.");

        if (!loan.IsActive)
            return OperationResult<Loan>.Failure($"Loan {loan.Id} has already been returned.");

        loan.Complete();
        loan.Item.Release();

        var message = $"{loan.Borrower.Name} returned {loan.Item.Id} ({loan.Item.CategoryName}).";
        var next = WaitingLists.Peek(loan.Item.Category);
        if (next is not null)
            message += $" Next in the waiting list: {next.DisplayName}.";

        return OperationResult<Loan>.Success(loan, message);
    }

    /// <summary>Adds a borrower to a category's waiting list.</summary>
    public OperationResult JoinWaitingList(string? borrowerId, EquipmentCategory category)
    {
        var label = EquipmentCatalog.Label(category);

        if (Inventory.ConfiguredCount(category) == 0)
            return OperationResult.Failure($"No {label} items were configured, so there is nothing to wait for.");

        var eligibility = _policy.CheckEligibility(borrowerId, category);
        if (!eligibility.Succeeded)
            return eligibility;

        var borrower = Borrowers.Find(borrowerId)!;

        if (WaitingLists.Contains(category, borrower.Id))
            return OperationResult.Failure($"{borrower.Name} is already waiting for a {label}.");

        var queue = WaitingLists.GetQueue(category);
        if (queue.Count == 0 && Inventory.AvailableCount(category) > 0)
            return OperationResult.Failure($"A {label} is available. Borrow it directly instead.");

        WaitingLists.Enqueue(category, borrower);
        return OperationResult.Success($"{borrower.Name} joined the {label} waiting list at position {queue.Count + 1}.");
    }

    /// <summary>Removes a borrower from a waiting list.</summary>
    public OperationResult CancelWaitingRequest(string? borrowerId, EquipmentCategory category)
    {
        var label = EquipmentCatalog.Label(category);
        var borrower = Borrowers.Find(borrowerId);

        if (borrower is null || !WaitingLists.Remove(category, borrower.Id))
            return OperationResult.Failure($"Select a borrower in the {label} waiting list to cancel.");

        return OperationResult.Success($"Cancelled {borrower.Name}'s request for a {label}.");
    }

    /// <summary>Lends a returned item to the next borrower in the queue.</summary>
    public OperationResult<Loan> IssueToNextWaiting(EquipmentCategory category)
    {
        var label = EquipmentCatalog.Label(category);

        var next = WaitingLists.Peek(category);
        if (next is null)
            return OperationResult<Loan>.Failure($"Nobody is waiting for a {label}.");

        var item = Inventory.FindAvailable(category);
        if (item is null)
            return OperationResult<Loan>.Failure($"No {label} has been returned yet.");

        var eligibility = _policy.CheckEligibility(next.Id, category);
        if (!eligibility.Succeeded)
            return OperationResult<Loan>.Failure($"{eligibility.Message} Cancel their request to serve the next borrower.");

        var result = Issue(next, item);
        WaitingLists.Dequeue(category);
        return result;
    }

    /// <summary>Gets borrowers who currently hold an item.</summary>
    public IReadOnlyList<Borrower> ActiveBorrowers() =>
        Loans.Active.Select(l => l.Borrower).Distinct().ToList();

    /// <summary>Creates a loan and assigns the item.</summary>
    private OperationResult<Loan> Issue(Borrower borrower, EquipmentItem item)
    {
        var loan = new Loan(_idGenerator.Next(LoanIdPrefix), borrower, item);
        item.AssignTo(loan);
        Loans.Record(loan);

        return OperationResult<Loan>.Success(
            loan,
            $"Loan {loan.Id}: {item.Id} ({item.CategoryName}) issued to {borrower.DisplayName}.");
    }
}
