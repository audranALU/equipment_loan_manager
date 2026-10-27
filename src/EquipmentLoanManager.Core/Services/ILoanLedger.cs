using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public interface ILoanLedger
{
    IReadOnlyList<Loan> Active { get; }

    IReadOnlyList<Loan> Completed { get; }

    void Record(Loan loan);

    Loan? Find(string? id);

    IReadOnlyList<Loan> ActiveFor(string borrowerId);
}
