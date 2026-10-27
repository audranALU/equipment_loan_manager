using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.Tests;

public class InventoryTests
{
    /// <summary>Checks only the entered quantities are created.</summary>
    [Fact]
    public void Constructor_CreatesOnlyTheEnteredQuantities()
    {
        var manager = TestSession.Create(laptops: 2, headsets: 3);

        Assert.Equal(5, manager.Inventory.TotalItems);
        Assert.Equal(2, manager.Inventory.ConfiguredCount(EquipmentCategory.Laptop));
        Assert.Equal(3, manager.Inventory.ConfiguredCount(EquipmentCategory.VrHeadset));
    }

    /// <summary>Checks every item gets a unique ID.</summary>
    [Fact]
    public void Constructor_GivesEveryItemAUniqueId()
    {
        var manager = TestSession.Create(5, 8, 10, 6, 12);

        var ids = manager.Inventory.Items.Select(i => i.Id).ToList();

        Assert.Equal(41, ids.Count);
        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    /// <summary>Checks each item has the right type.</summary>
    [Fact]
    public void Constructor_CreatesTheMatchingItemType()
    {
        var manager = TestSession.Create(cameras: 1, tripods: 1);

        Assert.Contains(manager.Inventory.Items, i => i is Camera360);
        Assert.Contains(manager.Inventory.Items, i => i is Tripod);
    }

    /// <summary>Checks quantities above the limit are rejected.</summary>
    [Fact]
    public void Constructor_RejectsQuantityAboveCapacity()
    {
        var quantities = new Dictionary<EquipmentCategory, int> { [EquipmentCategory.Camera360] = 6 };

        Assert.Throws<ArgumentOutOfRangeException>(() => new Inventory(quantities));
    }

    /// <summary>Checks the summary totals are correct.</summary>
    [Fact]
    public void Summarise_ReportsConfiguredAvailableAndBorrowed()
    {
        var manager = TestSession.Create(laptops: 3);
        var borrower = manager.Register("Amara");
        manager.Borrow(borrower.Id, EquipmentCategory.Laptop);

        var laptops = manager.Inventory.Summarise().Single(s => s.Category == EquipmentCategory.Laptop);

        Assert.Equal(3, laptops.Configured);
        Assert.Equal(2, laptops.Available);
        Assert.Equal(1, laptops.Borrowed);
    }
}
