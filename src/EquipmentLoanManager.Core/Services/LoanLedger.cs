using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Services;

public sealed class LoanLedger : ILoanLedger
{
    private readonly List<Loan> _loans = [];

    public IReadOnlyList<Loan> Active => _loans.Where(l => l.IsActive).ToList();

    public IReadOnlyList<Loan> Completed => _loans.Where(l => !l.IsActive).ToList();

    public void Record(Loan loan)
    {
        ArgumentNullException.ThrowIfNull(loan);
        _loans.Add(loan);
    }

    public Loan? Find(string? id) =>
        _loans.FirstOrDefault(l => string.Equals(l.Id, id?.Trim(), StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<Loan> ActiveFor(string borrowerId) =>
        _loans
            .Where(l => l.IsActive && string.Equals(l.Borrower.Id, borrowerId, StringComparison.OrdinalIgnoreCase))
            .ToList();
}
