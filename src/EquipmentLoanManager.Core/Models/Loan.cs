namespace EquipmentLoanManager.Core.Models;

public sealed class Loan
{
    /// <summary>Creates a loan of an item to a borrower.</summary>
    internal Loan(string id, Borrower borrower, EquipmentItem item)
    {
        Id = id;
        Borrower = borrower;
        Item = item;
    }

    public string Id { get; }

    public Borrower Borrower { get; }

    public EquipmentItem Item { get; }

    public LoanStatus Status { get; private set; } = LoanStatus.Active;

    public bool IsActive => Status == LoanStatus.Active;

    /// <summary>Marks the loan as returned.</summary>
    internal void Complete()
    {
        if (!IsActive)
            throw new InvalidOperationException($"Loan {Id} has already been returned.");

        Status = LoanStatus.Completed;
    }
}
