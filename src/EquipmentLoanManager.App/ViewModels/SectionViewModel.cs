using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

public abstract class SectionViewModel(LoanManager manager, Action<OperationResult> report) : ObservableObject
{
    protected LoanManager Manager { get; } = manager;

    /// <summary>Reloads the tab data.</summary>
    public abstract void Refresh();

    /// <summary>Sends a result to the status banner.</summary>
    protected void Report(OperationResult result) => report(result);
}
