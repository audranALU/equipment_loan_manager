using System.Windows.Input;
using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>View model for the loans tab.</summary>
public sealed class LoansViewModel : SectionViewModel
{
    private IReadOnlyList<Borrower> _borrowers = [];
    private Borrower? _selectedBorrower;
    private CategoryOption? _selectedCategory;
    private IReadOnlyList<Loan> _activeLoans = [];
    private IReadOnlyList<Loan> _completedLoans = [];
    private Loan? _selectedLoan;

    /// <summary>Creates the loans tab and its commands.</summary>
    public LoansViewModel(LoanManager manager, Action<OperationResult> report)
        : base(manager, report)
    {
        BorrowCommand = new RelayCommand(Borrow, () => SelectedBorrower is not null && SelectedCategory is not null);
        ReturnCommand = new RelayCommand(Return, () => SelectedLoan is not null);
    }

    /// <summary>The categories that can be borrowed.</summary>
    public IReadOnlyList<CategoryOption> Categories => CategoryOption.All;

    /// <summary>The borrowers who can borrow.</summary>
    public IReadOnlyList<Borrower> Borrowers
    {
        get => _borrowers;
        private set => SetProperty(ref _borrowers, value);
    }

    /// <summary>The selected borrower.</summary>
    public Borrower? SelectedBorrower
    {
        get => _selectedBorrower;
        set => SetProperty(ref _selectedBorrower, value);
    }

    /// <summary>The selected category.</summary>
    public CategoryOption? SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    /// <summary>Loans that have not been returned.</summary>
    public IReadOnlyList<Loan> ActiveLoans
    {
        get => _activeLoans;
        private set => SetProperty(ref _activeLoans, value);
    }

    /// <summary>Loans that have been returned.</summary>
    public IReadOnlyList<Loan> CompletedLoans
    {
        get => _completedLoans;
        private set => SetProperty(ref _completedLoans, value);
    }

    /// <summary>The selected active loan.</summary>
    public Loan? SelectedLoan
    {
        get => _selectedLoan;
        set => SetProperty(ref _selectedLoan, value);
    }

    /// <summary>Lends an item to the selected borrower.</summary>
    public ICommand BorrowCommand { get; }

    /// <summary>Returns the selected loan.</summary>
    public ICommand ReturnCommand { get; }

    /// <summary>Reloads borrowers, active loans and loan history.</summary>
    public override void Refresh()
    {
        var selected = SelectedBorrower;

        Borrowers = Manager.Borrowers.All.ToList();
        SelectedBorrower = selected is not null && Borrowers.Contains(selected) ? selected : null;
        ActiveLoans = Manager.Loans.Active;
        CompletedLoans = Manager.Loans.Completed;
    }

    /// <summary>Lends an item to the selected borrower.</summary>
    private void Borrow()
    {
        if (SelectedCategory is null)
            return;

        Report(Manager.Borrow(SelectedBorrower?.Id, SelectedCategory.Value));
    }

    /// <summary>Returns the selected loan.</summary>
    private void Return() => Report(Manager.Return(SelectedLoan?.Id));
}
