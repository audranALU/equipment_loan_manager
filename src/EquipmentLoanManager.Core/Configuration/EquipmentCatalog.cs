using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Configuration;

public static class EquipmentCatalog
{
    private static readonly IReadOnlyDictionary<EquipmentCategory, CategoryInfo> Categories =
        new Dictionary<EquipmentCategory, CategoryInfo>
        {
            [EquipmentCategory.Camera360] = new("360 Camera", "CAM", 5),
            [EquipmentCategory.Laptop] = new("Laptop", "LAP", 8),
            [EquipmentCategory.VrHeadset] = new("VR Headset", "VRH", 10),
            [EquipmentCategory.Microphone] = new("Microphone", "MIC", 6),
            [EquipmentCategory.Tripod] = new("Tripod", "TRI", 12)
        };

    public static IReadOnlyList<EquipmentCategory> All { get; } = Categories.Keys.ToList();

    public static CategoryInfo Get(EquipmentCategory category) => Categories[category];

    public static string Label(EquipmentCategory category) => Get(category).Label;

    public static int MaxCapacity(EquipmentCategory category) => Get(category).MaxCapacity;
}
