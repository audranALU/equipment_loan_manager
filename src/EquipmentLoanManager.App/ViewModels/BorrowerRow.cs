using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>A borrower row in the borrowers table.</summary>
public sealed record BorrowerRow(Borrower Borrower, int ActiveLoans, string WaitingFor);
