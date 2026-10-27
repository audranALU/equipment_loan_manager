using EquipmentLoanManager.Core.Configuration;

namespace EquipmentLoanManager.Core.Models;

/// <summary>A single piece of equipment.</summary>
public abstract class EquipmentItem
{
    /// <summary>Creates an item with the given ID.</summary>
    protected EquipmentItem(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        Id = id;
    }

    /// <summary>The item ID.</summary>
    public string Id { get; }

    /// <summary>The item category.</summary>
    public abstract EquipmentCategory Category { get; }

    /// <summary>The category display name.</summary>
    public string CategoryName => EquipmentCatalog.Label(Category);

    /// <summary>The loan holding this item, if any.</summary>
    public Loan? CurrentLoan { get; private set; }

    /// <summary>Whether the item is not on loan.</summary>
    public bool IsAvailable => CurrentLoan is null;

    /// <summary>Available or Borrowed.</summary>
    public string Status => IsAvailable ? "Available" : "Borrowed";

    /// <summary>The name of the borrower holding the item.</summary>
    public string BorrowerName => CurrentLoan?.Borrower.Name ?? string.Empty;

    /// <summary>Marks the item as on loan for the given loan.</summary>
    internal void AssignTo(Loan loan)
    {
        if (!IsAvailable)
            throw new InvalidOperationException($"{Id} is already on loan.");

        CurrentLoan = loan;
    }

    /// <summary>Marks the item as available again.</summary>
    internal void Release() => CurrentLoan = null;
}
