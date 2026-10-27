using EquipmentLoanManager.Core.Configuration;

namespace EquipmentLoanManager.Core.Models;

public abstract class EquipmentItem
{
    protected EquipmentItem(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        Id = id;
    }

    public string Id { get; }

    public abstract EquipmentCategory Category { get; }

    public string CategoryName => EquipmentCatalog.Label(Category);

    public Loan? CurrentLoan { get; private set; }

    public bool IsAvailable => CurrentLoan is null;

    public string Status => IsAvailable ? "Available" : "Borrowed";

    public string BorrowerName => CurrentLoan?.Borrower.Name ?? string.Empty;

    internal void AssignTo(Loan loan)
    {
        if (!IsAvailable)
            throw new InvalidOperationException($"{Id} is already on loan.");

        CurrentLoan = loan;
    }

    internal void Release() => CurrentLoan = null;
}
