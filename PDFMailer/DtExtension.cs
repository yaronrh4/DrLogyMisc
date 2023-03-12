using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDFMailer
{
    public static class DtExtension
    {
public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        object val = dr[column.ColumnName];
                        if (!(val is DBNull))
                            {
                            if (pro.PropertyType == typeof(int))
                                val = Convert.ToInt32(val);
                            else if (pro.PropertyType == typeof(float))
                                val = Convert.ToSingle(val);
                            else if (pro.PropertyType == typeof(decimal))
                                val = Convert.ToDecimal(val);

                            try
                            {
                                pro.SetValue(obj, val, null);
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> self)
        {
            var properties = typeof(T).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
                dataTable.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType)
                   ?? info.PropertyType);

            foreach (var entity in self)
                dataTable.Rows.Add(properties.Select(p => p.GetValue(entity)).ToArray());

            return dataTable;
        }

    }
}
