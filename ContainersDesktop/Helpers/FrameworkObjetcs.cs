using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;

namespace ContainersDesktop.Comunes.Helpers;
public static class FrameworkObjects
{
    public static List<FrameworkElement> Children(this DependencyObject parent)
    {
        var list = new List<FrameworkElement>();

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is FrameworkElement)
            {
                list.Add(child as FrameworkElement);
            }

            list.AddRange(Children(child));
        }

        return list;
    }
}
