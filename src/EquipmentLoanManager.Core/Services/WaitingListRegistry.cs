using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public sealed class WaitingListRegistry : IWaitingListRegistry
{
    private readonly Dictionary<EquipmentCategory, Queue<Borrower>> _queues =
        EquipmentCatalog.All.ToDictionary(category => category, _ => new Queue<Borrower>());

    public void Enqueue(EquipmentCategory category, Borrower borrower)
    {
        ArgumentNullException.ThrowIfNull(borrower);

        if (Contains(category, borrower.Id))
            throw new InvalidOperationException($"{borrower.DisplayName} is already waiting.");

        _queues[category].Enqueue(borrower);
    }

    public Borrower? Peek(EquipmentCategory category) =>
        _queues[category].TryPeek(out var borrower) ? borrower : null;

    public Borrower? Dequeue(EquipmentCategory category) =>
        _queues[category].TryDequeue(out var borrower) ? borrower : null;

    public bool Contains(EquipmentCategory category, string borrowerId) =>
        _queues[category].Any(b => Matches(b, borrowerId));

    public bool Remove(EquipmentCategory category, string borrowerId)
    {
        var queue = _queues[category];
        var remaining = queue.Where(b => !Matches(b, borrowerId)).ToList();

        if (remaining.Count == queue.Count)
            return false;

        _queues[category] = new Queue<Borrower>(remaining);
        return true;
    }

    public int RemoveEverywhere(string borrowerId) =>
        EquipmentCatalog.All.Count(category => Remove(category, borrowerId));

    public IReadOnlyList<Borrower> GetQueue(EquipmentCategory category) => _queues[category].ToList();

    private static bool Matches(Borrower borrower, string borrowerId) =>
        string.Equals(borrower.Id, borrowerId, StringComparison.OrdinalIgnoreCase);
}
