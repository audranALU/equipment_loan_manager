using System.Globalization;
using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Configuration;

public static class QuantityValidator
{
    /// <summary>Checks that the entered quantity is a whole number within the category limit.</summary>
    public static OperationResult<int> Validate(EquipmentCategory category, string? input)
    {
        var label = EquipmentCatalog.Label(category);
        var max = EquipmentCatalog.MaxCapacity(category);
        var text = input?.Trim() ?? string.Empty;

        if (text.Length == 0)
            return OperationResult<int>.Failure($"Enter a quantity for {label}.");

        if (!int.TryParse(text, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var quantity))
            return OperationResult<int>.Failure($"{label}: '{text}' is not a whole number.");

        if (quantity < 0 || quantity > max)
            return OperationResult<int>.Failure($"{label}: quantity must be between 0 and {max}.");

        return OperationResult<int>.Success(quantity, $"{label}: {quantity} configured.");
    }
}
