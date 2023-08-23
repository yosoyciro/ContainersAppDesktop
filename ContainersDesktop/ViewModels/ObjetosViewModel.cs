using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ContainersDesktop.ViewModels;

public class ObjetosViewModel : ObservableValidator
{
    #region Properties para dar de alta un container
    private string _matricula;

    public ObjetosViewModel()
    {
        ErrorsChanged += ObjetosVM_ErrorsChanged;
        PropertyChanged += ObjetosVM_PropertyChanged;
    }

    ~ObjetosViewModel()
    {
        ErrorsChanged -= ObjetosVM_ErrorsChanged;
        PropertyChanged -= ObjetosVM_PropertyChanged;
    }

    [Required(ErrorMessage = "La Matrícula es requerida")]
    [MinLength(5, ErrorMessage = "La Matrícula debe tener un mínimo de 5 caracteres")]
    public string Matricula
    {
        get => _matricula;
        set => SetProperty(ref _matricula, value, true);
    }
    
    public bool IsValid => Errors.Length == 0 && !string.IsNullOrEmpty(_matricula);

    public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);
    public ICommand ValidateCommand => new RelayCommand(() => ValidateAllProperties());

    #endregion

    #region Property value change
    private void ObjetosVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(HasErrors))
        {
            OnPropertyChanged(nameof(HasErrors));            
        }
    }

    private void ObjetosVM_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Errors));
        OnPropertyChanged(nameof(IsValid));
    }

    #endregion
}
