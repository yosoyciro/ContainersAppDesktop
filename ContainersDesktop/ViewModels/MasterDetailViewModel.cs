using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

public abstract partial class MasterDetailViewModel<T> : ObservableObject
{
    private string filter;
    private T current;
    private readonly ObservableCollection<T> items = new();
    
    public ObservableCollection<T> Items =>
        filter is null
            ? items
            : new ObservableCollection<T>(items.Where(i => ApplyFilter(i, filter)));

    public T Current
    {
        get => current;
        set
        {
            SetProperty(ref current, value);
            OnPropertyChanged(nameof(HasCurrent));
        }
    }

    public string Filter
    {
        get => filter;
        set
        {
            var current = Current;

            SetProperty(ref filter, value);
            OnPropertyChanged(nameof(Items));

            if (current is not null && Items.Contains(current))
            {
                Current = current;
            }
        }
    }

    public bool HasCurrent => current is not null;

    public abstract bool ApplyFilter(T item, string filter);

    // ...
    public virtual T AddItem(T item)
    {
        items.Add(item);
        if (filter is not null)
        {
            OnPropertyChanged(nameof(Items));
        }

        return item;
    }

    public virtual T UpdateItem(T item, T original)
    {
        var hasCurrent = HasCurrent;

        var i = items.IndexOf(original);
        items[i] = item; // Raises CollectionChanged.

        if (filter is not null)
        {
            OnPropertyChanged(nameof(Items));
        }

        if (hasCurrent && !HasCurrent)
        {
            // Restore Current.
            Current = item;
        }

        return item;
    }

    public virtual void DeleteItem(T item)
    {
        items.Remove(item);

        if (filter is not null)
        {
            OnPropertyChanged(nameof(Items));
        }
    }
}