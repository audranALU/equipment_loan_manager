namespace EquipmentLoanManager.Core.Services;

/// <summary>Generates unique IDs.</summary>
public interface IIdGenerator
{
    /// <summary>Returns the next unique ID for the given prefix.</summary>
    string Next(string prefix);
}
