using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//using BusinessLayer;
using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DataLayer.DatabaseEntites;
using System.Collections;

namespace DataLayer
{
    public class Mapper
    {
        public static T MapRow<T>(DataRow row)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            T entry = Activator.CreateInstance<T>();
            
            foreach (PropertyInfo property in properties)
            {
                object? value = row[GetName(property)];

                if (IsWhatever.IsForeignKey(property))
                {
                    MethodInfo genericmethod = typeof(DBConnection).GetMethods().Where(m => m.Name == "Select" && m.GetParameters().Length == 1).First().MakeGenericMethod(property.PropertyType);
                    //object? foreignkey = genericmethod.Invoke(null, new object[] { value });
                    var foreignObject = ((IEnumerable)genericmethod.Invoke(null, new object[] { value })).Cast<object>().ToList() ;
                    
                    property.SetValue(entry, foreignObject[0]);
                }
                else
                {
                    if (value != DBNull.Value)
                    {
                        var obj = Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                        property.SetValue(entry, obj);
                    }
                    else
                    {
                        property.SetValue(entry, null);
                    }
                }

                
                
            }
            Console.WriteLine(entry);
            return entry;
        }

        private static string GetName(PropertyInfo prop)
        {
            if(IsWhatever.IsName(prop, out string name))
                return name;
            return prop.Name;
        }


        

    }
    
}
