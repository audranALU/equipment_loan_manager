namespace EquipmentLoanManager.Core.Models;

/// <summary>An item lent to a borrower.</summary>
public sealed class Loan
{
    /// <summary>Creates a loan of an item to a borrower.</summary>
    internal Loan(string id, Borrower borrower, EquipmentItem item)
    {
        Id = id;
        Borrower = borrower;
        Item = item;
    }

    /// <summary>The loan ID.</summary>
    public string Id { get; }

    /// <summary>The borrower.</summary>
    public Borrower Borrower { get; }

    /// <summary>The item on loan.</summary>
    public EquipmentItem Item { get; }

    /// <summary>Whether the loan is active or completed.</summary>
    public LoanStatus Status { get; private set; } = LoanStatus.Active;

    /// <summary>Whether the item has not been returned yet.</summary>
    public bool IsActive => Status == LoanStatus.Active;

    /// <summary>Marks the loan as returned.</summary>
    internal void Complete()
    {
        if (!IsActive)
            throw new InvalidOperationException($"Loan {Id} has already been returned.");

        Status = LoanStatus.Completed;
    }
}
