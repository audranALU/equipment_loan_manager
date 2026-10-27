using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>View model for the main window.</summary>
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

    /// <summary>The inventory tab.</summary>
    public InventoryViewModel Inventory { get; }

    /// <summary>The borrowers tab.</summary>
    public BorrowersViewModel Borrowers { get; }

    /// <summary>The loans tab.</summary>
    public LoansViewModel Loans { get; }

    /// <summary>The waiting lists tab.</summary>
    public WaitingListsViewModel WaitingLists { get; }

    /// <summary>The total number of items.</summary>
    public int TotalItems => _manager.Inventory.TotalItems;

    /// <summary>The number of items not on loan.</summary>
    public int AvailableItems
    {
        get => _availableItems;
        private set => SetProperty(ref _availableItems, value);
    }

    /// <summary>The number of active loans.</summary>
    public int ActiveLoanCount
    {
        get => _activeLoanCount;
        private set => SetProperty(ref _activeLoanCount, value);
    }

    /// <summary>The number of borrowers.</summary>
    public int BorrowerCount
    {
        get => _borrowerCount;
        private set => SetProperty(ref _borrowerCount, value);
    }

    /// <summary>The message shown in the status banner.</summary>
    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    /// <summary>Whether the status message is an error.</summary>
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
