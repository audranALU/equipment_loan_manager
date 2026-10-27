using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public interface IInventory
{
    IReadOnlyList<EquipmentItem> Items { get; }

    int TotalItems { get; }

    int ConfiguredCount(EquipmentCategory category);

    int AvailableCount(EquipmentCategory category);

    EquipmentItem? FindAvailable(EquipmentCategory category);

    IReadOnlyList<CategorySummary> Summarise();
}
