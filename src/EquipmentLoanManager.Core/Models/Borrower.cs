namespace EquipmentLoanManager.Core.Models;

/// <summary>A person who can borrow equipment.</summary>
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

    /// <summary>The borrower ID.</summary>
    public string Id { get; }

    /// <summary>The borrower name.</summary>
    public string Name { get; }

    /// <summary>The name followed by the ID.</summary>
    public string DisplayName => $"{Name} ({Id})";

    /// <summary>Returns the borrower name and ID.</summary>
    public override string ToString() => DisplayName;
}
