namespace EquipmentLoanManager.Core.Models;

/// <summary>A 360 camera.</summary>
public sealed class Camera360(string id) : EquipmentItem(id)
{
    /// <summary>The item category.</summary>
    public override EquipmentCategory Category => EquipmentCategory.Camera360;
}

/// <summary>A laptop.</summary>
public sealed class Laptop(string id) : EquipmentItem(id)
{
    /// <summary>The item category.</summary>
    public override EquipmentCategory Category => EquipmentCategory.Laptop;
}

/// <summary>A VR headset.</summary>
public sealed class VrHeadset(string id) : EquipmentItem(id)
{
    /// <summary>The item category.</summary>
    public override EquipmentCategory Category => EquipmentCategory.VrHeadset;
}

/// <summary>A microphone.</summary>
public sealed class Microphone(string id) : EquipmentItem(id)
{
    /// <summary>The item category.</summary>
    public override EquipmentCategory Category => EquipmentCategory.Microphone;
}

/// <summary>A tripod.</summary>
public sealed class Tripod(string id) : EquipmentItem(id)
{
    /// <summary>The item category.</summary>
    public override EquipmentCategory Category => EquipmentCategory.Tripod;
}
