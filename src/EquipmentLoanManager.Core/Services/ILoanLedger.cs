using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

/// <summary>Keeps track of all loans.</summary>
public interface ILoanLedger
{
    /// <summary>Loans that have not been returned.</summary>
    IReadOnlyList<Loan> Active { get; }

    /// <summary>Loans that have been returned.</summary>
    IReadOnlyList<Loan> Completed { get; }

    /// <summary>Records a new loan.</summary>
    void Record(Loan loan);

    /// <summary>Finds a loan by ID.</summary>
    Loan? Find(string? id);

    /// <summary>Gets a borrower's active loans.</summary>
    IReadOnlyList<Loan> ActiveFor(string borrowerId);
}
