using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ContainersDesktop.Dominio.Models.Base;
public partial class BaseEntityObservable : ObservableObject
{
    [Key]
    [ObservableProperty]
    public int id;       
}
