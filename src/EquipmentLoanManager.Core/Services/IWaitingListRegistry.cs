using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public interface IWaitingListRegistry
{
    void Enqueue(EquipmentCategory category, Borrower borrower);

    Borrower? Peek(EquipmentCategory category);

    Borrower? Dequeue(EquipmentCategory category);

    bool Contains(EquipmentCategory category, string borrowerId);

    bool Remove(EquipmentCategory category, string borrowerId);

    int RemoveEverywhere(string borrowerId);

    IReadOnlyList<Borrower> GetQueue(EquipmentCategory category);
}
