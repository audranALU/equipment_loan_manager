using System.Windows.Input;
using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>View model for the borrowers tab.</summary>
public sealed class BorrowersViewModel : SectionViewModel
{
    private string _newName = string.Empty;
    private string _newId = string.Empty;
    private IReadOnlyList<BorrowerRow> _rows = [];
    private BorrowerRow? _selectedRow;

    /// <summary>Creates the borrowers tab and its commands.</summary>
    public BorrowersViewModel(LoanManager manager, Action<OperationResult> report)
        : base(manager, report)
    {
        AddCommand = new RelayCommand(Add);
        RemoveCommand = new RelayCommand(Remove, () => SelectedRow is not null);
    }

    /// <summary>The name entered for a new borrower.</summary>
    public string NewName
    {
        get => _newName;
        set => SetProperty(ref _newName, value);
    }

    /// <summary>The optional ID entered for a new borrower.</summary>
    public string NewId
    {
        get => _newId;
        set => SetProperty(ref _newId, value);
    }

    /// <summary>The borrowers shown in the table.</summary>
    public IReadOnlyList<BorrowerRow> Rows
    {
        get => _rows;
        private set => SetProperty(ref _rows, value);
    }

    /// <summary>The selected borrower row.</summary>
    public BorrowerRow? SelectedRow
    {
        get => _selectedRow;
        set => SetProperty(ref _selectedRow, value);
    }

    /// <summary>Adds a borrower.</summary>
    public ICommand AddCommand { get; }

    /// <summary>Removes the selected borrower.</summary>
    public ICommand RemoveCommand { get; }

    /// <summary>Reloads the borrower list.</summary>
    public override void Refresh()
    {
        var selectedId = SelectedRow?.Borrower.Id;

        Rows = Manager.Borrowers.All
            .Select(b => new BorrowerRow(
                b,
                Manager.Loans.ActiveFor(b.Id).Count,
                string.Join(", ", EquipmentCatalog.All
                    .Where(c => Manager.WaitingLists.Contains(c, b.Id))
                    .Select(EquipmentCatalog.Label))))
            .ToList();

        SelectedRow = Rows.FirstOrDefault(r => r.Borrower.Id == selectedId);
    }

    /// <summary>Adds a borrower from the entered name and ID.</summary>
    private void Add()
    {
        var result = Manager.AddBorrower(NewName, NewId);
        if (result.Succeeded)
        {
            NewName = string.Empty;
            NewId = string.Empty;
        }

        Report(result);
    }

    /// <summary>Removes the selected borrower.</summary>
    private void Remove() => Report(Manager.RemoveBorrower(SelectedRow?.Borrower.Id));
}
