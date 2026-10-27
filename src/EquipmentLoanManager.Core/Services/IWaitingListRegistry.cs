using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public interface IWaitingListRegistry
{
    /// <summary>Adds a borrower to the end of a category queue.</summary>
    void Enqueue(EquipmentCategory category, Borrower borrower);

    /// <summary>Gets the borrower at the front of a queue without removing them.</summary>
    Borrower? Peek(EquipmentCategory category);

    /// <summary>Removes and returns the borrower at the front of a queue.</summary>
    Borrower? Dequeue(EquipmentCategory category);

    /// <summary>Checks whether a borrower is waiting in a queue.</summary>
    bool Contains(EquipmentCategory category, string borrowerId);

    /// <summary>Removes a borrower from a queue.</summary>
    bool Remove(EquipmentCategory category, string borrowerId);

    /// <summary>Removes a borrower from every queue.</summary>
    int RemoveEverywhere(string borrowerId);

    /// <summary>Gets a category queue in order.</summary>
    IReadOnlyList<Borrower> GetQueue(EquipmentCategory category);
}
