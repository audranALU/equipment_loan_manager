namespace EquipmentLoanManager.Core.Services;

public sealed class SequentialIdGenerator : IIdGenerator
{
    private readonly Dictionary<string, int> _counters = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>Returns the next numbered ID for the given prefix.</summary>
    public string Next(string prefix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);

        var next = _counters.GetValueOrDefault(prefix) + 1;
        _counters[prefix] = next;
        return $"{prefix}-{next:D3}";
    }
}
