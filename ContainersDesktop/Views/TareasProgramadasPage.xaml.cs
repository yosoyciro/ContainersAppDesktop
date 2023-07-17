using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using ContainersDesktop.ViewModels;
using Microsoft.UI.Xaml.Controls;
using ContainersDesktop.Core.Helpers;
using ContainersDesktop.DTO;
using CommunityToolkit.WinUI.UI.Controls;

namespace ContainersDesktop.Views;

public sealed partial class TareasProgramadasPage : Page
{
    public TareasProgramadasViewModel ViewModel
    {
        get;
    }
    public TareasProgramadasPage()
    {
        ViewModel = App.GetService<TareasProgramadasViewModel>();
        this.InitializeComponent();
    }
   
    private void TareasProgramadasGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        // Add sorting indicator, and sort
        var isAscending = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending;
        TareasProgramadasGrid.ItemsSource = ViewModel.SortData(e.Column.Tag.ToString(), isAscending);
        e.Column.SortDirection = isAscending
            ? DataGridSortDirection.Ascending
            : DataGridSortDirection.Descending;

        // Remove sorting indicators from other columns
        foreach (var column in TareasProgramadasGrid.Columns)
        {
            if (column.Tag != null && column.Tag.ToString() != e.Column.Tag.ToString())
            {
                column.SortDirection = null;
            }
        }
    }
}
