using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

public sealed record QueueEntry(int Position, Borrower Borrower);

public sealed record QueueOverview(string Label, int Count, string Order);
