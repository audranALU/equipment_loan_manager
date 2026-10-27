using EquipmentLoanManager.App.Infrastructure;
using EquipmentLoanManager.Core.Results;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App.ViewModels;

public abstract class SectionViewModel(LoanManager manager, Action<OperationResult> report) : ObservableObject
{
    protected LoanManager Manager { get; } = manager;

    public abstract void Refresh();

    protected void Report(OperationResult result) => report(result);
}
