namespace EquipmentLoanManager.Core.Services;

public interface IIdGenerator
{
    string Next(string prefix);
}
