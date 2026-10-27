namespace EquipmentLoanManager.Core.Models;

public sealed class Camera360(string id) : EquipmentItem(id)
{
    public override EquipmentCategory Category => EquipmentCategory.Camera360;
}

public sealed class Laptop(string id) : EquipmentItem(id)
{
    public override EquipmentCategory Category => EquipmentCategory.Laptop;
}

public sealed class VrHeadset(string id) : EquipmentItem(id)
{
    public override EquipmentCategory Category => EquipmentCategory.VrHeadset;
}

public sealed class Microphone(string id) : EquipmentItem(id)
{
    public override EquipmentCategory Category => EquipmentCategory.Microphone;
}

public sealed class Tripod(string id) : EquipmentItem(id)
{
    public override EquipmentCategory Category => EquipmentCategory.Tripod;
}
