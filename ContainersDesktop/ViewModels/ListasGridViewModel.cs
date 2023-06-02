using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using ContainersDesktop.Contracts.ViewModels;
using ContainersDesktop.Core.Contracts.Services;
using ContainersDesktop.Core.Models;

namespace ContainersDesktop.ViewModels;

public partial class ListasGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public ListasGridViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
