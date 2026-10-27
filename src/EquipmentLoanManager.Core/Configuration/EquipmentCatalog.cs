using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.Core.Configuration;

/// <summary>The fixed list of categories and their limits.</summary>
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

    /// <summary>All categories.</summary>
    public static IReadOnlyList<EquipmentCategory> All { get; } = Categories.Keys.ToList();

    /// <summary>Gets the settings for a category.</summary>
    public static CategoryInfo Get(EquipmentCategory category) => Categories[category];

    /// <summary>Gets the display name of a category.</summary>
    public static string Label(EquipmentCategory category) => Get(category).Label;

    /// <summary>Gets the maximum stock allowed for a category.</summary>
    public static int MaxCapacity(EquipmentCategory category) => Get(category).MaxCapacity;
}
