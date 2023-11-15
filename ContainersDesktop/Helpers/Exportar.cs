using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using ContainersDesktop.Comunes.Helpers;

namespace ContainersDesktop.Helpers;
public static class Exportar
{
    public static ICommand AbrirUbicacionCommand => new RelayCommand(AbrirUbicacionCommand_Execute);

    public static async Task GenerarDatos<T>(ObservableCollection<T> lista, string filePath, XamlRoot root)
    {
        StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write));

        DataSet ds = lista.ConvertToDataSet("table");

        DataTable dt = ds.Tables[0];

        int iColCount = dt.Columns.Count;
        for (int i = 0; i < iColCount; i++)
        {
            sw.Write(dt.Columns[i]);
            if (i < iColCount - 1)
            {
                sw.Write(",");
            }
        }
        sw.Write(sw.NewLine);
        // Now write all the rows.  
        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < iColCount; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    sw.Write(dr[i].ToString());
                }
                if (i < iColCount - 1)
                {
                    sw.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                }
            }
            sw.Write(sw.NewLine);
        }
        sw.Close();

        ContentDialog exportarDialog = new ContentDialog
        {
            XamlRoot = root,
            Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
            Title = Constantes.exportarDialog_Title.GetLocalized(),
            Content = string.Format(Constantes.exportarDialog_Content.GetLocalized(), filePath),
            PrimaryButtonText = Constantes.exportarDialog_PrimaryButtonText.GetLocalized(),
            PrimaryButtonCommand = AbrirUbicacionCommand,
            CloseButtonText = Constantes.exportarDialog_CloseButtonText.GetLocalized(),
        };

        await exportarDialog.ShowAsync();
    }

    //public static void GenerarDatos<T>(ObservableCollection<T> lista, string filePath)
    //{
    //    StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.Write));

    //    DataSet ds = lista.ConvertToDataSet("table");

    //    DataTable dt = ds.Tables[0];

    //    int iColCount = dt.Columns.Count;
    //    for (int i = 0; i < iColCount; i++)
    //    {
    //        sw.Write(dt.Columns[i]);
    //        if (i < iColCount - 1)
    //        {
    //            sw.Write(",");
    //        }
    //    }
    //    sw.Write(sw.NewLine);
    //    // Now write all the rows.  
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        for (int i = 0; i < iColCount; i++)
    //        {
    //            if (!Convert.IsDBNull(dr[i]))
    //            {
    //                sw.Write(dr[i].ToString());
    //            }
    //            if (i < iColCount - 1)
    //            {
    //                sw.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
    //            }
    //        }
    //        sw.Write(sw.NewLine);
    //    }
    //    sw.Close();
    //}

    public static DataSet ConvertToDataSet<T>(this IEnumerable<T> source, string name)
    {
        if (source == null)
            throw new ArgumentNullException("source ");
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException("name");
        var converted = new DataSet(name);
        converted.Tables.Add(NewTable(name, source));
        return converted;
    }

    private static DataTable NewTable<T>(string name, IEnumerable<T> list)
    {
        PropertyInfo[] propInfo = typeof(T).GetProperties();
        DataTable table = Table<T>(name, list, propInfo);
        IEnumerator<T> enumerator = list.GetEnumerator();
        while (enumerator.MoveNext())
            table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, propInfo));
        return table;
    }

    private static DataRow CreateRow<T>(DataRow row, T listItem, PropertyInfo[] pi)
    {
        foreach (PropertyInfo p in pi)
            row[p.Name.ToString()] = p.GetValue(listItem, null);
        return row;
    }

    private static DataTable Table<T>(string name, IEnumerable<T> list, PropertyInfo[] pi)
    {
        DataTable table = new DataTable(name);
        foreach (PropertyInfo p in pi)
            table.Columns.Add(p.Name, p.PropertyType);
        return table;
    }

    private static void AbrirUbicacionCommand_Execute()
    {
        ArchivosCarpetas.AbrirUbicacion(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));
    }
}
