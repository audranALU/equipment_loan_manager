namespace EquipmentLoanManager.Core.Models;

public sealed class Loan
{
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

    internal void Complete()
    {
        if (!IsActive)
            throw new InvalidOperationException($"Loan {Id} has already been returned.");

        Status = LoanStatus.Completed;
    }
}
