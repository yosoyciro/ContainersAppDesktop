﻿using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace ContainersDesktop.Core.Helpers;
public static class Exportar
{
    public static void GenerarDatos<T>(ObservableCollection<T> lista, string filePath)
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
    }

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
}
