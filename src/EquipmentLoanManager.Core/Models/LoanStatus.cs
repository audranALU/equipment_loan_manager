namespace EquipmentLoanManager.Core.Models;

/// <summary>The state of a loan.</summary>
public enum LoanStatus
{
    /// <summary>The item is still on loan.</summary>
    Active,

    /// <summary>The item has been returned.</summary>
    Completed
}
