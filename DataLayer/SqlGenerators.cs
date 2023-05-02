using DataLayer.DatabaseEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLayer
{
    internal static class SqlGenerators
    {
        //todo sql string generators and refactoring 
        //but it isnt time :(

        public static string GetName(PropertyInfo prop)
        {
            if (IsWhatever.IsName(prop, out string name))
                return name;
            return prop.Name;
        }


        public static string GetSqlType(PropertyInfo prop)
        {
            Type type = prop.PropertyType;

            if (type.IsGenericType)
                type = type.GetGenericArguments()[0];


            switch (type)
            {
                case Type t when t == typeof(int):
                    return "INTEGER";
                case Type t when t == typeof(string):
                    return "TEXT";
                case Type t when t == typeof(double):
                    return "REAL";
                case Type t when t == typeof(bool):
                    return "INTEGER";
                case Type t when t == typeof(DateTime):
                    return "TEXT";
                case Type t when t == typeof(User):
                    return "INTEGER";
                case Type t when t == typeof(Place):
                    return "INTEGER";
                case Type t when t == typeof(long):
                    return "INTEGER";
                default:
                    throw new Exception("Unknown type");
            }

        }

       

    }
}
