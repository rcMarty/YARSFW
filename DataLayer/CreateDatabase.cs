using DataLayer.DatabaseEntites;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal class CreateDB
    {

        public static void CreateTable<T>(string connectionString)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            //making querry to create generic table
            StringBuilder sql = new StringBuilder();
            sql.Append($"CREATE TABLE IF NOT EXISTS {type.Name} (");
            foreach (var item in properties)
                sql.Append($"{GetProperty(item)}, ");

            foreach (var item in properties)
                sql.Append($"{GetAdditionalKeys(item)}");

            sql.Remove(sql.Length - 2, 2);
            sql.Append(");");


            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine(sql.ToString());
                SqliteCommand command = new SqliteCommand(sql.ToString(), conn);
                command.ExecuteNonQuery();
            }
        }

        

        private static string GetProperty(PropertyInfo prop)
        {
            return SqlGenerators.GetName(prop) + " " + SqlGenerators.GetSqlType(prop) + " " + ((IsWhatever.IsNullable(prop)) ? "null" : "not null");
        }

        public static string GetAdditionalKeys(PropertyInfo prop)
        {
            StringBuilder ret = new StringBuilder();


            if (IsWhatever.IsPrimaryKey(prop))
            {
                ret.Append($"primary key ({SqlGenerators.GetName(prop)}), ");
            }
            else if (IsWhatever.IsForeignKey(prop, out string table))
            {
                //getting name of id which table should be connected
                var customproperties = prop.PropertyType.GetProperties();
                PropertyInfo? anotherID = null;
                foreach (var itemprop in customproperties)
                {
                    if (itemprop.GetCustomAttribute<DBPrimaryKeyAttribute>() != null)
                    {
                        anotherID = itemprop;
                        anotherID = itemprop;
                        break;
                    }
                }
                if (anotherID == null)
                    throw new Exception("no primary id found for " + prop);



                // make string
                ret.Append($"foreign key ({SqlGenerators.GetName(prop)}) " +
                    $"references {table} ({SqlGenerators.GetName(anotherID)}) " +
                    $"on delete cascade on update no action, ");

            }


            return ret.ToString();
        }

    }
}
