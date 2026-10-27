using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public interface IInventory
{
    IReadOnlyList<EquipmentItem> Items { get; }

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
