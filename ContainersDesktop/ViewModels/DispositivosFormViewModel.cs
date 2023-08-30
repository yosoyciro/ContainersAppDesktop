using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using ContainersDesktop.Dominio.DTO;
using ContainersDesktop.Dominio.Models;

namespace ContainersDesktop.ViewModels;

public class DispositivosFormViewModel : ObservableValidator
{
    private string _descripcion;
    private string _container;
    private bool _containerValido;

    public DispositivosFormViewModel()
    {
        ErrorsChanged += VMErrorsChanged;
        PropertyChanged += VMPropertyChanged;
    }

    ~DispositivosFormViewModel()
    {
        ErrorsChanged -= VMErrorsChanged;
        PropertyChanged -= VMPropertyChanged;
    }

    [Required(ErrorMessage = "La Descripcion es requerida")]
    [MinLength(1, ErrorMessage = "La Descripcion debe tener un mínimo de 1 caracter")]
    public string Descripcion
    {
        get => _descripcion;
        set => SetProperty(ref _descripcion, value, true);
    }

    [Required(ErrorMessage = "El Container es requerido")]
    [MinLength(6, ErrorMessage = "El container debe contener 6 caracteres (3 letras y 3 números)")]
    public string Container
    {
        get => _container;
        set => SetProperty(ref _container, value, true);
    }

    [CustomValidation(typeof(DispositivosFormViewModel), nameof(ValidarContainer))]
    public bool ContainerValido
    {
        get => _containerValido;
        set => SetProperty(ref _containerValido, value, true);
    }

    public bool IsValid => Errors.Length == 0;

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

    public static ValidationResult ValidarContainer(Dispositivo value, ValidationContext context)
    {
        var instance = (DispositivosFormViewModel)context.ObjectInstance;
        if (instance?.ContainerValido == null)
        {
            return null;
        }

        var isValid = instance.ContainerValido;

        if (isValid)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("El Container no está creado");
        }
    }
}
