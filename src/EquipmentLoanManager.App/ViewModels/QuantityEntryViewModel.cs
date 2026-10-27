using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>A quantity input for one category.</summary>
public sealed class QuantityEntryViewModel(EquipmentCategory category) : ObservableObject
{
    private string _input = "0";
    private string _error = string.Empty;

    /// <summary>The category this entry is for.</summary>
    public EquipmentCategory Category { get; } = category;

    /// <summary>The category display name.</summary>
    public string Label { get; } = EquipmentCatalog.Label(category);

    /// <summary>The allowed range shown to the user.</summary>
    public string Hint { get; } = $"0 to {EquipmentCatalog.MaxCapacity(category)}";

    /// <summary>The text the user entered.</summary>
    public string Input
    {
        get => _input;
        set
        {
            if (SetProperty(ref _input, value))
                Error = string.Empty;
        }
    }

    /// <summary>The validation error, if any.</summary>
    public string Error
    {
        get => _error;
        private set => SetProperty(ref _error, value);
    }

    /// <summary>Validates the input and shows an error if it is invalid.</summary>
    public bool TryGetQuantity(out int quantity)
    {
        var result = QuantityValidator.Validate(Category, Input);
        quantity = result.Value;
        Error = result.Succeeded ? string.Empty : result.Message;
        return result.Succeeded;
    }
}
