using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.DTO;
using Microsoft.WindowsAppSDK.Runtime;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ContainersDesktop.ViewModels;

public class TareasProgramadasFormViewModel : ObservableValidator
{
    private ObjetosDTO _objeto;
    private DateTimeOffset _fechaProgramada;
    private TimeSpan _horaProgramada;
    private AlmacenesDTO _ubicacionOrigen;
    private AlmacenesDTO _ubicacionDestino;
    private DispositivosDTO _dispositivo;
    private List<ValidationResult> _errors = new();

    public TareasProgramadasFormViewModel()
    {
        //ErrorsChanged += VMErrorsChanged;
        //PropertyChanged += VMPropertyChanged;
    }

    //~TareasProgramadasFormViewModel()
    //{
    //    ErrorsChanged -= VMErrorsChanged;
    //    PropertyChanged -= VMPropertyChanged;
    //}  

    [Required(ErrorMessage = "El Container es requerido")]
    public ObjetosDTO Objeto
    {
        get => _objeto;
        set => SetProperty(ref _objeto, value, true);
    }

    [Required(ErrorMessage = "La Fecha Programada es requerida")]
    public DateTimeOffset FechaProgramada
    {
        get => _fechaProgramada;
        set => SetProperty(ref _fechaProgramada, value, true);
    }

    [Required(ErrorMessage = "La Hora Programada es requerida")]
    public TimeSpan HoraProgramada
    {
        get => _horaProgramada;
        set => SetProperty(ref _horaProgramada, value, true);
    }

    //[Required(ErrorMessage = "La Ubicación Orígen es requerida")]
    [CustomValidation(typeof(TareasProgramadasFormViewModel), nameof(ValidateUbicacionOrigen))]
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
    [CustomValidation(typeof(TareasProgramadasFormViewModel), nameof(ValidateUbicacionDestino))]
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

    public string Errors => string.Join(Environment.NewLine, from ValidationResult e in _errors select e.ErrorMessage);
   

    #region Property value change
    //private void VMPropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //    if (e.PropertyName != nameof(HasErrors))
    //    {
    //        OnPropertyChanged(nameof(HasErrors));
    //    }
    //}

    //private void VMErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    //{
    //    OnPropertyChanged(nameof(Errors));
    //    OnPropertyChanged(nameof(IsValid));
    //}

    #endregion

    public static ValidationResult ValidateUbicacionOrigen(AlmacenesDTO value, ValidationContext context)
    {
        var instance = (TareasProgramadasFormViewModel)context.ObjectInstance;
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
        var instance = (TareasProgramadasFormViewModel)context.ObjectInstance;
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

    //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //    return null;
    //}
}
