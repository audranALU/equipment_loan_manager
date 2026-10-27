using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public interface ILoanLedger
{
    IReadOnlyList<Loan> Active { get; }

    IReadOnlyList<Loan> Completed { get; }

    /// <summary>Records a new loan.</summary>
    void Record(Loan loan);

    /// <summary>Finds a loan by ID.</summary>
    Loan? Find(string? id);

    /// <summary>Gets a borrower's active loans.</summary>
    IReadOnlyList<Loan> ActiveFor(string borrowerId);
}
