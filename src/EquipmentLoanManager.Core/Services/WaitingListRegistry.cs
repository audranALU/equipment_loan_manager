using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public sealed class WaitingListRegistry : IWaitingListRegistry
{
    private readonly Dictionary<EquipmentCategory, Queue<Borrower>> _queues =
        EquipmentCatalog.All.ToDictionary(category => category, _ => new Queue<Borrower>());

    /// <summary>Adds a borrower to the end of a category queue.</summary>
    public void Enqueue(EquipmentCategory category, Borrower borrower)
    {
        ArgumentNullException.ThrowIfNull(borrower);

        if (Contains(category, borrower.Id))
            throw new InvalidOperationException($"{borrower.DisplayName} is already waiting.");

        _queues[category].Enqueue(borrower);
    }

    /// <summary>Gets the borrower at the front of a queue without removing them.</summary>
    public Borrower? Peek(EquipmentCategory category) =>
        _queues[category].TryPeek(out var borrower) ? borrower : null;

    /// <summary>Removes and returns the borrower at the front of a queue.</summary>
    public Borrower? Dequeue(EquipmentCategory category) =>
        _queues[category].TryDequeue(out var borrower) ? borrower : null;

    /// <summary>Checks whether a borrower is waiting in a queue.</summary>
    public bool Contains(EquipmentCategory category, string borrowerId) =>
        _queues[category].Any(b => Matches(b, borrowerId));

    /// <summary>Removes a borrower from a queue.</summary>
    public bool Remove(EquipmentCategory category, string borrowerId)
    {
        var queue = _queues[category];
        var remaining = queue.Where(b => !Matches(b, borrowerId)).ToList();

        if (remaining.Count == queue.Count)
            return false;

        _queues[category] = new Queue<Borrower>(remaining);
        return true;
    }

    /// <summary>Removes a borrower from every queue.</summary>
    public int RemoveEverywhere(string borrowerId) =>
        EquipmentCatalog.All.Count(category => Remove(category, borrowerId));

    /// <summary>Gets a category queue in order.</summary>
    public IReadOnlyList<Borrower> GetQueue(EquipmentCategory category) => _queues[category].ToList();

    /// <summary>Checks whether a borrower has the given ID.</summary>
    private static bool Matches(Borrower borrower, string borrowerId) =>
        string.Equals(borrower.Id, borrowerId, StringComparison.OrdinalIgnoreCase);
}
