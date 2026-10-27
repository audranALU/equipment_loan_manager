using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

/// <summary>Stores the borrowers added in this session.</summary>
public interface IBorrowerRegistry
{
    /// <summary>All borrowers.</summary>
    IReadOnlyList<Borrower> All { get; }

    /// <summary>Adds a borrower, generating an ID if none is given.</summary>
    OperationResult<Borrower> Add(string? name, string? id = null);

    /// <summary>Removes a borrower by ID.</summary>
    bool Remove(string id);

    /// <summary>Finds a borrower by ID.</summary>
    Borrower? Find(string? id);
}
