﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.ViewModels;

public class ObjetosViewModel : ObservableValidator
{
    #region Properties para dar de alta un container
    private string _matricula;

    [Required(ErrorMessage = "La Matrícula es requerida")]
    [MinLength(5, ErrorMessage = "La Matrícula debe tener un mínimo de 5 caracteres")]
    public string Matricula
    {
        get => _matricula;
        set => SetProperty(ref _matricula, value, true);
    }

    public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);

    #endregion

    public ObjetosViewModel()
    {
        ErrorsChanged += Matricula_ErrorsChanged;
        PropertyChanged += Matricula_PropertyChanged;
    }

    ~ObjetosViewModel()
    {
        ErrorsChanged -= Matricula_ErrorsChanged;
        PropertyChanged -= Matricula_PropertyChanged;
    }

    #region Property value change
    private void Matricula_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(HasErrors))
        {
            OnPropertyChanged(nameof(HasErrors));
        }
    }

    private void Matricula_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
    {
        OnPropertyChanged(nameof(Errors));
    }
    #endregion
}
