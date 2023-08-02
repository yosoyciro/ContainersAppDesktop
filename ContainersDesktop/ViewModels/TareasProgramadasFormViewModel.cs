using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.DTO;

namespace ContainersDesktop.ViewModels;

public class TareasProgramadasFormViewModel : ObservableValidator
{
    private ObjetosDTO _objeto;
    private DateTimeOffset? _fechaProgramada;
    private AlmacenesDTO _ubicacionOrigen;
    private AlmacenesDTO _ubicacionDestino;
    private DispositivosDTO _dispositivo;

    public TareasProgramadasFormViewModel()
    {
        ErrorsChanged += VMErrorsChanged;
        PropertyChanged += VMPropertyChanged;
    }

    ~TareasProgramadasFormViewModel()
    {
        ErrorsChanged -= VMErrorsChanged;
        PropertyChanged -= VMPropertyChanged;
    }

    [Required(ErrorMessage = "El Container es requerido")]
    public ObjetosDTO Objeto
    {
        get => _objeto;
        set => SetProperty(ref _objeto, value, true);
    }

    [Required(ErrorMessage = "La Fecha Programada es requerida")]
    public DateTimeOffset? FechaProgramada
    {
        get => _fechaProgramada;
        set => SetProperty(ref _fechaProgramada, value, true);
    }

    [CustomValidation(typeof(TareasProgramadasFormViewModel), nameof(ValidateValue))]
    [Required(ErrorMessage = "La Ubicación Orígen es requerida")]
    public AlmacenesDTO UbicacionOrigen
    {
        get => _ubicacionOrigen;
        set => SetProperty(ref _ubicacionOrigen, value, true);
    }

    [Required(ErrorMessage = "La Ubicación Orígen es requerida")]
    [CustomValidation(typeof(TareasProgramadasFormViewModel), nameof(ValidateValue))]
    public AlmacenesDTO UbicacionDestino
    {
        get => _ubicacionDestino;
        set => SetProperty(ref _ubicacionDestino, value, true);
    }

    [Required(ErrorMessage = "El Dispositivo es requerido")]
    public DispositivosDTO Dispositivo
    {
        get => _dispositivo;
        set => SetProperty(ref _dispositivo, value, true);
    }

    public bool IsValid => Errors.Length == 0 ? true : false;

    public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);

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

    public static ValidationResult ValidateValue(AlmacenesDTO value, ValidationContext context)
    {
        var instance = (TareasProgramadasFormViewModel)context.ObjectInstance;

        if (instance.UbicacionDestino == null)
        {
            return ValidationResult.Success;
        }

        var isValid = value.MOVIM_ALMACEN != instance.UbicacionOrigen.MOVIM_ALMACEN;

        if (isValid)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult("La Ubicación de Orígen es igual a la de Destino");
    }
}
