using System.Collections.ObjectModel;

namespace ContainersDesktop.Comunes.Helpers;
public class SafeObservableCollection<T> : ObservableCollection<T>
{
    /// <summary>
    /// Normal ObservableCollection fails if you are trying to clear ObservableCollection<ObservableCollection<T>> if there is data inside
    /// this is workaround till it won't be fixed in Xamarin Forms
    /// </summary>
    protected override void ClearItems()
    {
        while (this.Items.Any())
        {
            this.Items.RemoveAt(0);
        }
    }
}
