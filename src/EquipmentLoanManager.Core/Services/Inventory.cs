using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public sealed class Inventory : IInventory
{
    private readonly List<EquipmentItem> _items = [];

    public Inventory(IReadOnlyDictionary<EquipmentCategory, int> quantities)
    {
        ArgumentNullException.ThrowIfNull(quantities);

        foreach (var category in EquipmentCatalog.All)
        {
            var info = EquipmentCatalog.Get(category);
            var quantity = quantities.GetValueOrDefault(category);

            if (quantity < 0 || quantity > info.MaxCapacity)
                throw new ArgumentOutOfRangeException(
                    nameof(quantities),
                    $"{info.Label} quantity must be between 0 and {info.MaxCapacity}.");

            for (var number = 1; number <= quantity; number++)
                _items.Add(EquipmentFactory.Create(category, $"{info.IdPrefix}-{number:D2}"));
        }
    }

    public IReadOnlyList<EquipmentItem> Items => _items;

    public int TotalItems => _items.Count;

    public int ConfiguredCount(EquipmentCategory category) =>
        _items.Count(item => item.Category == category);

    public int AvailableCount(EquipmentCategory category) =>
        _items.Count(item => item.Category == category && item.IsAvailable);

    public EquipmentItem? FindAvailable(EquipmentCategory category) =>
        _items.FirstOrDefault(item => item.Category == category && item.IsAvailable);

    public IReadOnlyList<CategorySummary> Summarise() =>
        EquipmentCatalog.All
            .Select(category =>
            {
                var configured = ConfiguredCount(category);
                var available = AvailableCount(category);
                return new CategorySummary(
                    category,
                    EquipmentCatalog.Label(category),
                    EquipmentCatalog.MaxCapacity(category),
                    configured,
                    available,
                    configured - available);
            })
            .ToList();
}
