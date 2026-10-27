using System.Windows.Input;
using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>View model for the stock configuration window.</summary>
public sealed class ConfigurationViewModel : ObservableObject
{
    private string _summary = string.Empty;

    /// <summary>Creates an entry for each category.</summary>
    public ConfigurationViewModel()
    {
        Entries = EquipmentCatalog.All.Select(c => new QuantityEntryViewModel(c)).ToList();
        StartCommand = new RelayCommand(Start);
    }

    /// <summary>Raised with the quantities once all entries are valid.</summary>
    public event Action<IReadOnlyDictionary<EquipmentCategory, int>>? Completed;

    /// <summary>The quantity entries, one per category.</summary>
    public IReadOnlyList<QuantityEntryViewModel> Entries { get; }

    /// <summary>The validation summary message.</summary>
    public string Summary
    {
        get => _summary;
        private set => SetProperty(ref _summary, value);
    }

    /// <summary>Validates the entries and starts the session.</summary>
    public ICommand StartCommand { get; }

    /// <summary>Validates all entries and starts the session if they are valid.</summary>
    private void Start()
    {
        var quantities = new Dictionary<EquipmentCategory, int>();
        var invalid = 0;

        foreach (var entry in Entries)
        {
            if (entry.TryGetQuantity(out var quantity))
                quantities[entry.Category] = quantity;
            else
                invalid++;
        }

        if (invalid > 0)
        {
            Summary = $"Fix {invalid} invalid entr{(invalid == 1 ? "y" : "ies")} and try again.";
            return;
        }

        Completed?.Invoke(quantities);
    }
}
