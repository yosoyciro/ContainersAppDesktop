using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.DTO;

namespace ContainersDesktop.ViewModels;

public class MovimientosFormViewModel : ObservableValidator
{
    private DateTimeOffset? _fecha;
    private TimeSpan _hora;
    private AlmacenesDTO _ubicacionOrigen;
    private AlmacenesDTO _ubicacionDestino;
    private DispositivosDTO _dispositivo;
    private List<ValidationResult> _errors = new();

    //public MovimientosFormViewModel()
    //{
    //    ErrorsChanged += VMErrorsChanged;
    //    PropertyChanged += VMPropertyChanged;
    //}

    //~MovimientosFormViewModel()
    //{
    //    ErrorsChanged -= VMErrorsChanged;
    //    PropertyChanged -= VMPropertyChanged;
    //}

    [Required(ErrorMessage = "La Fecha es requerida")]
    public DateTimeOffset? Fecha
    {
        get => _fecha;
        set => SetProperty(ref _fecha, value, true);
    }

    public TimeSpan Hora
    {
        get => _hora;
        set => SetProperty(ref _hora, value, true);
    }

    [CustomValidation(typeof(MovimientosFormViewModel), nameof(ValidateUbicacionOrigen))]
    public AlmacenesDTO UbicacionOrigen
    {
        get => _ubicacionOrigen;
        set
        {
            _errors.RemoveAll(v => v.ErrorMessage.Contains("Ubicación"));
            if (_ubicacionOrigen != null)
            {
                TrySetProperty(ref _ubicacionOrigen, value, out IReadOnlyCollection<ValidationResult> errors);
                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged(nameof(IsNotValid));
            }
            else
            {
                SetProperty(ref _ubicacionOrigen, value, true);
            }
        }
    }

    //[Required(ErrorMessage = "La Ubicación Destino es requerida")]
    [CustomValidation(typeof(MovimientosFormViewModel), nameof(ValidateUbicacionDestino))]
    public AlmacenesDTO UbicacionDestino
    {
        get => _ubicacionDestino;
        set
        {
            _errors.RemoveAll(v => v.ErrorMessage.Contains("Ubicación"));
            if (_ubicacionDestino != null)
            {
                TrySetProperty(ref _ubicacionDestino, value, out IReadOnlyCollection<ValidationResult> errors);
                _errors.AddRange(errors);
                OnPropertyChanged(nameof(Errors));
                OnPropertyChanged(nameof(IsValid));
                OnPropertyChanged(nameof(IsNotValid));
            }
            else
            {
                SetProperty(ref _ubicacionDestino, value, true);
            }
        }
    }

    [Required(ErrorMessage = "El Dispositivo es requerido")]
    public DispositivosDTO Dispositivo
    {
        get => _dispositivo;
        set => SetProperty(ref _dispositivo, value, true);
    }

    public bool IsValid => Errors.Length == 0;

    public bool IsNotValid => !IsValid;

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

    public static ValidationResult ValidateUbicacionOrigen(AlmacenesDTO value, ValidationContext context)
    {
        var instance = (MovimientosFormViewModel)context.ObjectInstance;
        if (instance?.UbicacionDestino == null)
        {
            return null;
        }

        var isValid = value != instance?.UbicacionDestino;

        if (isValid)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("La Ubicación de Orígen es igual a la de Destino");
        }
    }

    public static ValidationResult ValidateUbicacionDestino(AlmacenesDTO value, ValidationContext context)
    {
        var instance = (MovimientosFormViewModel)context.ObjectInstance;
        var isValid = value != instance?.UbicacionOrigen;

        if (isValid)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("La Ubicación de Destino es igual a la de Orígen");
        }
    }
}
