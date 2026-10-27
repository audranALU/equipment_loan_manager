using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>View model for the inventory tab.</summary>
public sealed class InventoryViewModel(LoanManager manager, Action<OperationResult> report)
    : SectionViewModel(manager, report)
{
    private IReadOnlyList<CategorySummary> _summaries = [];
    private IReadOnlyList<EquipmentItem> _items = [];

    public IReadOnlyList<CategorySummary> Summaries
    {
        get => _summaries;
        private set => SetProperty(ref _summaries, value);
    }

    public IReadOnlyList<EquipmentItem> Items
    {
        get => _items;
        private set => SetProperty(ref _items, value);
    }

    /// <summary>Reloads the stock totals and item list.</summary>
    public override void Refresh()
    {
        Summaries = Manager.Inventory.Summarise();
        Items = Manager.Inventory.Items.ToList();
    }
}
