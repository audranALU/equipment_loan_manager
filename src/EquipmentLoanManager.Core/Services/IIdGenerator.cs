namespace EquipmentLoanManager.Core.Services;

public interface IIdGenerator
{
    /// <summary>Returns the next unique ID for the given prefix.</summary>
    string Next(string prefix);
}
