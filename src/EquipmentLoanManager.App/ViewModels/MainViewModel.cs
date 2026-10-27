using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

public sealed class MainViewModel : ObservableObject
{
    private readonly LoanManager _manager;
    private readonly IReadOnlyList<SectionViewModel> _sections;
    private string _statusMessage = string.Empty;
    private bool _isError;
    private int _availableItems;
    private int _activeLoanCount;
    private int _borrowerCount;

    /// <summary>Creates the main window view model and its tabs.</summary>
    public MainViewModel(LoanManager manager)
    {
        _manager = manager;
        Inventory = new InventoryViewModel(manager, Show);
        Borrowers = new BorrowersViewModel(manager, Show);
        Loans = new LoansViewModel(manager, Show);
        WaitingLists = new WaitingListsViewModel(manager, Show);
        _sections = [Inventory, Borrowers, Loans, WaitingLists];

        Show(OperationResult.Success($"Session ready with {manager.Inventory.TotalItems} configured item(s)."));
    }

    public InventoryViewModel Inventory { get; }

    public BorrowersViewModel Borrowers { get; }

    public LoansViewModel Loans { get; }

    public WaitingListsViewModel WaitingLists { get; }

    public int TotalItems => _manager.Inventory.TotalItems;

    public int AvailableItems
    {
        get => _availableItems;
        private set => SetProperty(ref _availableItems, value);
    }

    public int ActiveLoanCount
    {
        get => _activeLoanCount;
        private set => SetProperty(ref _activeLoanCount, value);
    }

    public int BorrowerCount
    {
        get => _borrowerCount;
        private set => SetProperty(ref _borrowerCount, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public bool IsError
    {
        get => _isError;
        private set => SetProperty(ref _isError, value);
    }

    /// <summary>Shows a result in the banner and refreshes all tabs.</summary>
    private void Show(OperationResult result)
    {
        StatusMessage = result.Message;
        IsError = !result.Succeeded;

        foreach (var section in _sections)
            section.Refresh();

        AvailableItems = _manager.Inventory.Items.Count(i => i.IsAvailable);
        ActiveLoanCount = _manager.Loans.Active.Count;
        BorrowerCount = _manager.Borrowers.All.Count;
    }
}
