using EquipmentLoanManager.Core.Models;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.Tests;

internal static class TestSession
{
    /// <summary>Creates a test session with the given stock.</summary>
    public static LoanManager Create(
        int cameras = 0,
        int laptops = 0,
        int headsets = 0,
        int microphones = 0,
        int tripods = 0) =>
        LoanManager.Create(new Dictionary<EquipmentCategory, int>
        {
            [EquipmentCategory.Camera360] = cameras,
            [EquipmentCategory.Laptop] = laptops,
            [EquipmentCategory.VrHeadset] = headsets,
            [EquipmentCategory.Microphone] = microphones,
            [EquipmentCategory.Tripod] = tripods
        });

    /// <summary>Adds a borrower and returns it.</summary>
    public static Borrower Register(this LoanManager manager, string name) =>
        manager.AddBorrower(name).Value!;
}
