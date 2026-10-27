namespace EquipmentLoanManager.Core.Models;

public static class EquipmentFactory
{
    /// <summary>Creates an item of the given category.</summary>
    public static EquipmentItem Create(EquipmentCategory category, string id) => category switch
    {
        EquipmentCategory.Camera360 => new Camera360(id),
        EquipmentCategory.Laptop => new Laptop(id),
        EquipmentCategory.VrHeadset => new VrHeadset(id),
        EquipmentCategory.Microphone => new Microphone(id),
        EquipmentCategory.Tripod => new Tripod(id),
        _ => throw new ArgumentOutOfRangeException(nameof(category), category, "Unknown category.")
    };
}
