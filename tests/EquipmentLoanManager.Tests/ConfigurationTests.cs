using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Tests;

public class ConfigurationTests
{
    /// <summary>Checks each category has the correct limit.</summary>
    [Theory]
    [InlineData(EquipmentCategory.Camera360, 5)]
    [InlineData(EquipmentCategory.Laptop, 8)]
    [InlineData(EquipmentCategory.VrHeadset, 10)]
    [InlineData(EquipmentCategory.Microphone, 6)]
    [InlineData(EquipmentCategory.Tripod, 12)]
    public void Catalog_StoresCapacityLimitForEachCategory(EquipmentCategory category, int expected)
    {
        Assert.Equal(expected, EquipmentCatalog.MaxCapacity(category));
    }

    /// <summary>Checks valid quantities are accepted.</summary>
    [Theory]
    [InlineData("0", 0)]
    [InlineData("3", 3)]
    [InlineData(" 8 ", 8)]
    public void Validate_AcceptsWholeNumbersWithinLimit(string input, int expected)
    {
        var result = QuantityValidator.Validate(EquipmentCategory.Laptop, input);

        Assert.True(result.Succeeded);
        Assert.Equal(expected, result.Value);
    }

    /// <summary>Checks invalid quantities are rejected.</summary>
    [Theory]
    [InlineData("9")]
    [InlineData("-1")]
    [InlineData("2.5")]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_RejectsInvalidEntries(string? input)
    {
        var result = QuantityValidator.Validate(EquipmentCategory.Laptop, input);

        Assert.False(result.Succeeded);
        Assert.NotEmpty(result.Message);
    }
}
