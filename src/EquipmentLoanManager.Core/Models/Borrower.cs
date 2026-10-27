namespace EquipmentLoanManager.Core.Models;

public sealed class Borrower
{
    /// <summary>Creates a borrower with the given ID and name.</summary>
    public Borrower(string id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Id = id.Trim();
        Name = name.Trim();
    }

    public string Id { get; }

    public string Name { get; }

    public string DisplayName => $"{Name} ({Id})";

    /// <summary>Returns the borrower name and ID.</summary>
    public override string ToString() => DisplayName;
}
