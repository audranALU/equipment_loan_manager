using EquipmentLoanManager.Core.Models;

namespace EquipmentLoanManager.App.ViewModels;

/// <summary>A borrower and their position in a queue.</summary>
public sealed record QueueEntry(int Position, Borrower Borrower);

/// <summary>A summary of one category queue.</summary>
public sealed record QueueOverview(string Label, int Count, string Order);
