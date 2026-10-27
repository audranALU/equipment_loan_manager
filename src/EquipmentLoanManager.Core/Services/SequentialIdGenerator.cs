namespace EquipmentLoanManager.Core.Services;

public sealed class SequentialIdGenerator : IIdGenerator
{
    private readonly Dictionary<string, int> _counters = new(StringComparer.OrdinalIgnoreCase);

    public string Next(string prefix)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prefix);

        var next = _counters.GetValueOrDefault(prefix) + 1;
        _counters[prefix] = next;
        return $"{prefix}-{next:D3}";
    }
}
