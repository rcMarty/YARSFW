using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DataLayer
{
    public class DBPrimaryKeyAttribute : Attribute
    {
    }


    public class DBForeignKeyAttribute : Attribute
    {
        public string Table { get; set; }
        public DBForeignKeyAttribute(string table)
        {
            Table = table;
        }
    }
    
    public class DBIgnoreAttribute : Attribute
    {
    }

    public class DBIsNullableAttribute : Attribute
    {
    }


    
    

    public class DBNameAttribute : Attribute
    {
        public string Name { get; set; }
        
        public DBNameAttribute(string name)
        {
            Name = name;
        }
        
    }

    public static class IsWhatever
    {
        /// <summary>
        /// returns name of the given property if it has given attribute 
        /// else null
        /// </summary>
        /// <typeparam name="T"> Attribute</typeparam>
        /// <param name="property"> PropertyInfo </param>
        /// <param name="outproperty"> Name of the Property</param>
        /// <returns>property.Name</returns>
        public static bool IsAttribute<T>(PropertyInfo[] property, out List<PropertyInfo> outproperty) where T : Attribute
        {
            outproperty = new List<PropertyInfo>();
            foreach (var prop in property)
            {
                if (prop.GetCustomAttribute<T>() != null)
                {
                    outproperty.Add(prop);
                    
                }
            }
            if(outproperty.Count > 0)
            {
                return true;
            }

            
            return false;
        }
        /// <summary>
        /// returns type of the given property if it has given attribute 
        /// else null
        /// </summary>
        /// <typeparam name="T">Attribute</typeparam>
        /// <param name="property">PropertyInfo</param>
        /// <param name="outproperty"> property.Type</param>
        /// <returns>true if property has attribute</returns>
        public static bool IsAttribute<T>(PropertyInfo property, out object? outproperty) where T : Attribute
        {

            var tmp = property.GetCustomAttribute<T>();
            if (tmp != null)
            {
                outproperty = property.GetType();
                return true;
            }
            outproperty = null;
            return false;
        }

        public static bool IsPrimaryKey(PropertyInfo property)
        {
            return property.GetCustomAttribute<DBPrimaryKeyAttribute>() != null;
        }
        public static bool IsForeignKey(PropertyInfo property)
        {
            return property.GetCustomAttribute<DBForeignKeyAttribute>() != null;
        }

        public static bool IsForeignKey(PropertyInfo property, out string table)
        {
            var tmp = property.GetCustomAttribute<DBForeignKeyAttribute>();
            if (tmp != null)
            {
                table = tmp.Table;
                return true;
            }
            table = null;
            return false;
        }
        
        public static bool IsIgnore(PropertyInfo property)
        {
            return property.GetCustomAttribute<DBIgnoreAttribute>() != null;
        }

        public static bool IsNullable(PropertyInfo property)
        {
            return property.GetCustomAttribute<DBIsNullableAttribute>() != null;
        }

        public static bool IsName(PropertyInfo property, out string name)
        {
            string? customname = property.GetCustomAttribute<DBNameAttribute>()?.Name;
            if (string.IsNullOrEmpty(customname))
            {
                name = null;
                return false;
            }

            name = customname;
            return true;
        }

        public static bool IsGetPrimaryKey(PropertyInfo[] properties,out PropertyInfo? property)
        {
            foreach (var item in properties)
            {
                if (IsPrimaryKey(item))
                {
                    property = item;
                    return true;
                }
            }

            property = null;
            return false;
        }

    }

}
