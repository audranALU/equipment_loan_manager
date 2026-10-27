using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EquipmentLoanManager.App.Infrastructure;

/// <summary>Base class that notifies the UI when properties change.</summary>
public abstract class ObservableObject : INotifyPropertyChanged
{
    /// <summary>Raised when a property value changes.</summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Notifies the UI that a property changed.</summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    /// <summary>Updates a field and notifies the UI if the value changed.</summary>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
