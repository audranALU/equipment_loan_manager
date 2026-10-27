using System.Windows.Input;
using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>View model for the waiting lists tab.</summary>
public sealed class WaitingListsViewModel : SectionViewModel
{
    private CategoryOption _selectedCategory = CategoryOption.All[0];
    private IReadOnlyList<Borrower> _borrowers = [];
    private Borrower? _selectedBorrower;
    private IReadOnlyList<QueueEntry> _queue = [];
    private QueueEntry? _selectedEntry;
    private IReadOnlyList<QueueOverview> _overview = [];
    private string _nextInLine = string.Empty;

    /// <summary>Creates the waiting lists tab and its commands.</summary>
    public WaitingListsViewModel(LoanManager manager, Action<OperationResult> report)
        : base(manager, report)
    {
        JoinCommand = new RelayCommand(Join, () => SelectedBorrower is not null);
        CancelCommand = new RelayCommand(Cancel, () => SelectedEntry is not null);
        IssueCommand = new RelayCommand(Issue, () => Queue.Count > 0);
    }

    /// <summary>The categories that have a queue.</summary>
    public IReadOnlyList<CategoryOption> Categories => CategoryOption.All;

    /// <summary>The selected category.</summary>
    public CategoryOption SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            if (SetProperty(ref _selectedCategory, value))
                Refresh();
        }
    }

    /// <summary>The borrowers who can join a queue.</summary>
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

    /// <summary>The queue for the selected category.</summary>
    public IReadOnlyList<QueueEntry> Queue
    {
        get => _queue;
        private set => SetProperty(ref _queue, value);
    }

    /// <summary>The selected queue entry.</summary>
    public QueueEntry? SelectedEntry
    {
        get => _selectedEntry;
        set => SetProperty(ref _selectedEntry, value);
    }

    /// <summary>A summary of every queue.</summary>
    public IReadOnlyList<QueueOverview> Overview
    {
        get => _overview;
        private set => SetProperty(ref _overview, value);
    }

    /// <summary>Who is next in the selected queue.</summary>
    public string NextInLine
    {
        get => _nextInLine;
        private set => SetProperty(ref _nextInLine, value);
    }

    /// <summary>Adds the selected borrower to the queue.</summary>
    public ICommand JoinCommand { get; }

    /// <summary>Removes the selected entry from the queue.</summary>
    public ICommand CancelCommand { get; }

    /// <summary>Issues an item to the next borrower.</summary>
    public ICommand IssueCommand { get; }

    /// <summary>Reloads the borrowers, the selected queue and the queue overview.</summary>
    public override void Refresh()
    {
        var category = SelectedCategory.Value;
        var selected = SelectedBorrower;

        Borrowers = Manager.Borrowers.All.ToList();
        SelectedBorrower = selected is not null && Borrowers.Contains(selected) ? selected : null;

        Queue = Manager.WaitingLists.GetQueue(category)
            .Select((borrower, index) => new QueueEntry(index + 1, borrower))
            .ToList();

        Overview = EquipmentCatalog.All
            .Select(c =>
            {
                var queue = Manager.WaitingLists.GetQueue(c);
                return new QueueOverview(
                    EquipmentCatalog.Label(c),
                    queue.Count,
                    queue.Count == 0 ? "-" : string.Join("  >  ", queue.Select(b => b.Name)));
            })
            .ToList();

        NextInLine = DescribeNext(category);
    }

    /// <summary>Describes who is next in a category queue.</summary>
    private string DescribeNext(EquipmentCategory category)
    {
        var next = Manager.WaitingLists.Peek(category);
        if (next is null)
            return "Nobody is waiting.";

        var available = Manager.Inventory.AvailableCount(category);
        return available > 0
            ? $"Next: {next.DisplayName}. An item is ready to issue."
            : $"Next: {next.DisplayName}. Waiting for a return.";
    }

    /// <summary>Adds the selected borrower to the selected queue.</summary>
    private void Join() => Report(Manager.JoinWaitingList(SelectedBorrower?.Id, SelectedCategory.Value));

    /// <summary>Removes the selected entry from the queue.</summary>
    private void Cancel() => Report(Manager.CancelWaitingRequest(SelectedEntry?.Borrower.Id, SelectedCategory.Value));

    /// <summary>Issues an item to the next borrower in the queue.</summary>
    private void Issue() => Report(Manager.IssueToNextWaiting(SelectedCategory.Value));
}
