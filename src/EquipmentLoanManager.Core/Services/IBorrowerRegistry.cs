using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

public interface IBorrowerRegistry
{
    IReadOnlyList<Borrower> All { get; }

    /// <summary>Adds a borrower, generating an ID if none is given.</summary>
    OperationResult<Borrower> Add(string? name, string? id = null);

    /// <summary>Removes a borrower by ID.</summary>
    bool Remove(string id);

    /// <summary>Finds a borrower by ID.</summary>
    Borrower? Find(string? id);
}
