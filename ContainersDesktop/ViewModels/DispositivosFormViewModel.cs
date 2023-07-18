using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.ViewModels;

public class DispositivosFormViewModel : ObservableValidator
{
    private string _descripcion;
    private string _container;

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
    [MinLength(6, ErrorMessage = "El container debe contener 6 caracteres")]
    public string Container
    {
        get => _container;
        set => SetProperty(ref _container, value, true);
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
