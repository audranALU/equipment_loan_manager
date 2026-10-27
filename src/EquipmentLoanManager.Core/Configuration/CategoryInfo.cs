namespace EquipmentLoanManager.Core.Configuration;

/// <summary>The label, ID prefix and stock limit of a category.</summary>
public sealed record CategoryInfo(string Label, string IdPrefix, int MaxCapacity);
