using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

/// <summary>Holds the configured equipment items.</summary>
public interface IInventory
{
    /// <summary>All items.</summary>
    IReadOnlyList<EquipmentItem> Items { get; }

    /// <summary>The total number of items.</summary>
    int TotalItems { get; }

    /// <summary>Counts all items in a category.</summary>
    int ConfiguredCount(EquipmentCategory category);

    /// <summary>Counts the items in a category that are not on loan.</summary>
    int AvailableCount(EquipmentCategory category);

    /// <summary>Finds the first available item in a category.</summary>
    EquipmentItem? FindAvailable(EquipmentCategory category);

    /// <summary>Returns stock totals for every category.</summary>
    IReadOnlyList<CategorySummary> Summarise();
}
