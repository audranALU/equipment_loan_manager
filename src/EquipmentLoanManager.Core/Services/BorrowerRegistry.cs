using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Results;

namespace EquipmentLoanManager.Core.Services;

/// <summary>Stores the borrowers added in this session.</summary>
public sealed class BorrowerRegistry(IIdGenerator idGenerator) : IBorrowerRegistry
{
    private const string IdPrefix = "BR";

    private readonly List<Borrower> _borrowers = [];

    public IReadOnlyList<Borrower> All => _borrowers;

    /// <summary>Adds a borrower, generating an ID if none is given.</summary>
    public OperationResult<Borrower> Add(string? name, string? id = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return OperationResult<Borrower>.Failure("Borrower name cannot be empty.");

        var borrowerId = string.IsNullOrWhiteSpace(id) ? GenerateId() : id.Trim();

        if (Find(borrowerId) is not null)
            return OperationResult<Borrower>.Failure($"A borrower with ID {borrowerId} already exists.");

        var borrower = new Borrower(borrowerId, name);
        _borrowers.Add(borrower);
        return OperationResult<Borrower>.Success(borrower, $"Added {borrower.DisplayName}.");
    }

    /// <summary>Removes a borrower by ID.</summary>
    public bool Remove(string id)
    {
        var borrower = Find(id);
        return borrower is not null && _borrowers.Remove(borrower);
    }

    /// <summary>Finds a borrower by ID.</summary>
    public Borrower? Find(string? id) =>
        string.IsNullOrWhiteSpace(id)
            ? null
            : _borrowers.FirstOrDefault(b => string.Equals(b.Id, id.Trim(), StringComparison.OrdinalIgnoreCase));

    /// <summary>Generates an ID that is not already in use.</summary>
    private string GenerateId()
    {
        string id;
        do
        {
            id = idGenerator.Next(IdPrefix);
        }
        while (Find(id) is not null);

        return id;
    }
}
