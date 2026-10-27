using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public sealed class LoanLedger : ILoanLedger
{
    private readonly List<Loan> _loans = [];

    public IReadOnlyList<Loan> Active => _loans.Where(l => l.IsActive).ToList();

    public IReadOnlyList<Loan> Completed => _loans.Where(l => !l.IsActive).ToList();

    /// <summary>Records a new loan.</summary>
    public void Record(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        _loans.Add(loan);
    }

    /// <summary>Finds a loan by ID.</summary>
    public Loan? Find(string? id) =>
        _loans.FirstOrDefault(l => string.Equals(l.Id, id?.Trim(), StringComparison.OrdinalIgnoreCase));

    /// <summary>Gets a borrower's active loans.</summary>
    public IReadOnlyList<Loan> ActiveFor(string borrowerId) =>
        _loans
            .Where(l => l.IsActive && string.Equals(l.Borrower.Id, borrowerId, StringComparison.OrdinalIgnoreCase))
            .ToList();
}
