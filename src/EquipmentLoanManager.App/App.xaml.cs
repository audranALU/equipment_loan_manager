using System.Windows;
using EquipmentLoanManager.App.ViewModels;
using EquipmentLoanManager.App.Views;
using EquipmentLoanManager.Core.Services;

namespace EquipmentLoanManager.App;

public partial class App : Application
{
    /// <summary>Shows the configuration window, then opens the main window.</summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var configuration = new ConfigurationViewModel();
        var configurationWindow = new ConfigurationWindow { DataContext = configuration };

        configuration.Completed += quantities =>
        {
            var main = new MainWindow { DataContext = new MainViewModel(LoanManager.Create(quantities)) };
            MainWindow = main;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            main.Show();
            configurationWindow.Close();
        };

        configurationWindow.Closed += (_, _) =>
        {
            if (MainWindow is not Views.MainWindow)
                Shutdown();
        };

        configurationWindow.Show();
    }
}
