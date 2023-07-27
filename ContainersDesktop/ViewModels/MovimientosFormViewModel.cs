using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.ViewModels;

public class MovimientosFormViewModel : ObservableValidator
{
    private DateTimeOffset? _fecha;

    public MovimientosFormViewModel()
    {
        ErrorsChanged += VMErrorsChanged;
        PropertyChanged += VMPropertyChanged;
    }

    ~MovimientosFormViewModel()
    {
        ErrorsChanged -= VMErrorsChanged;
        PropertyChanged -= VMPropertyChanged;
    }

    [Required(ErrorMessage = "La Fecha es requerida")]
    public DateTimeOffset? Fecha
    {
        get => _fecha;
        set => SetProperty(ref _fecha, value, true);
    }

    public bool IsValid => Errors.Length == 0;

    public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    //public ICommand ValidateCommand => new RelayCommand(() => ValidateAllProperties());

    #region Property value change
    private void VMPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(HasErrors))
        {
            OnPropertyChanged(nameof(HasErrors));
        }
    }

    private void VMErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(IsValid));
    }

    #endregion
}
