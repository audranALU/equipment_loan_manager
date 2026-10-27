using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

/// <summary>Decides whether a borrower may borrow.</summary>
public interface IBorrowingPolicy
{
    /// <summary>Checks whether a borrower can take an item from a category.</summary>
    OperationResult CheckEligibility(string? borrowerId, EquipmentCategory category);
}
