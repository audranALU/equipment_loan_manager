namespace EquipmentLoanManager.Core.Models;

public sealed record CategorySummary(
    EquipmentCategory Category,
    string Label,
    int MaxCapacity,
    int Configured,
    int Available,
    int Borrowed);
