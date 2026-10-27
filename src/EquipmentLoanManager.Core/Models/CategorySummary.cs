namespace EquipmentLoanManager.Core.Models;

/// <summary>Stock totals for one category.</summary>
public sealed record CategorySummary(
    EquipmentCategory Category,
    string Label,
    int MaxCapacity,
    int Configured,
    int Available,
    int Borrowed);
