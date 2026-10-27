using EquipmentLoanManager.Core.Configuration;
using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

public sealed record CategoryOption(EquipmentCategory Value, string Label)
{
    public static IReadOnlyList<CategoryOption> All { get; } =
        EquipmentCatalog.All.Select(c => new CategoryOption(c, EquipmentCatalog.Label(c))).ToList();

    /// <summary>Returns the category label.</summary>
    public override string ToString() => Label;
}
