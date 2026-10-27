using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

public sealed class QuantityEntryViewModel(EquipmentCategory category) : ObservableObject
{
    private string _input = "0";
    private string _error = string.Empty;

    public EquipmentCategory Category { get; } = category;

    public string Label { get; } = EquipmentCatalog.Label(category);

    public string Hint { get; } = $"0 to {EquipmentCatalog.MaxCapacity(category)}";

    public string Input
    {
        get => _input;
        set
        {
            if (SetProperty(ref _input, value))
                Error = string.Empty;
        }
    }

    public string Error
    {
        get => _error;
        private set => SetProperty(ref _error, value);
    }

    public bool TryGetQuantity(out int quantity)
    {
        var result = QuantityValidator.Validate(Category, Input);
        quantity = result.Value;
        Error = result.Succeeded ? string.Empty : result.Message;
        return result.Succeeded;
    }
}
