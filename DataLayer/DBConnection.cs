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
using System.Drawing;
using System.Collections;

namespace DataLayer
{
    public class DBConnection
    {
        //private const string connString = "Data Source=database.db;";
        //private const string connString = "Data Source=C:\\Users\\Marty\\OneDrive - VSB-TUO\\Dokumenty\\_Škola\\4_Semestr\\C#\\projekt\\DataLayer\\DesktopApp\\bin\\Debug\\net6.0-windows\\database.db"; //this is for desktop
        //private const string connString = "Data Source=C:\\Users\\rc_marty\\OneDrive - VSB-TUO\\Dokumenty\\_Škola\\4_Semestr\\C#\\projekt\\DataLayer\\DesktopApp\\bin\\Debug\\net6.0-windows\\database.db"; //this is for notebook
        private static string connString = "Data Source=" + DBConfig.ConnectionString;

        public static void CreateDatabase()
        {
            CreateTable<Place>();
            CreateTable<User>();
            CreateTable<Reservation>();
        }

        public static void CreateTable<T>()
        {
            CreateDB.CreateTable<T>(connString);
        }
        public static void Insert(object InObject)
        {
            
            Type type = InObject.GetType();
            PropertyInfo[] properties = type.GetProperties();


            if (IsWhatever.IsGetPrimaryKey(properties, out PropertyInfo? id1) && id1.GetValue(InObject) != null)
            {
                Console.WriteLine(id1.GetValue(InObject));
                throw new Exception("you want to insert object which already has ID");
            }

            //making querry to insert generic object
            StringBuilder sql = new StringBuilder();
            sql.Append($"INSERT INTO { type.Name } (");
            foreach (var item in properties)
            {
                if (IsWhatever.IsPrimaryKey(item))
                {
                    continue;
                }
                if (IsWhatever.IsForeignKey(item))
                {
                    
                    if (IsWhatever.IsGetPrimaryKey(item.GetValue(InObject).GetType().GetProperties(), out PropertyInfo? id2) && id2.GetValue(item.GetValue(InObject)) != null)
                    {
                        sql.Append($"{SqlGenerators.GetName(id2)}, ");
                    }
                    else
                    {
                        Insert(item.GetValue(InObject));
                        sql.Append($"{SqlGenerators.GetName(item)}, ");
                    }
                    continue;

                }
                
                if (IsWhatever.IsNullable(item) && item.GetValue(InObject) == null)
                {
                    continue;
                }
                if (IsWhatever.IsIgnore(item))
                {
                    continue;
                }


                sql.Append($"{item.Name}, ");
            }
            sql.Remove(sql.Length - 2, 2);
            sql.Append(") VALUES (");
            foreach (var item in properties)
            {
                //insert values into sqllquerry
                if (IsWhatever.IsPrimaryKey(item))
                {
                    continue;
                }
                if (IsWhatever.IsAttribute<DBForeignKeyAttribute>(item, out object itemType))
                {
                    sql.Append($"@{SqlGenerators.GetName(item)}, ");
                    continue;
                    
                }
                if (IsWhatever.IsNullable(item) && item.GetValue(InObject) == null)
                {
                    continue;
                }
                if (IsWhatever.IsIgnore(item))
                {
                    continue;
                }
                sql.Append($"@{SqlGenerators.GetName(item)}, ");

            }
            sql.Remove(sql.Length - 2, 2);
            sql.Append(");");
            
            Console.WriteLine(sql);
            
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(sql.ToString(), conn);
                foreach (var item in properties)
                {
                    if (IsWhatever.IsForeignKey(item))  
                    {
                        IsWhatever.IsGetPrimaryKey(item.GetValue(InObject).GetType().GetProperties(), out PropertyInfo? property);
                        command.Parameters.AddWithValue($"@{SqlGenerators.GetName(item)}", property.GetValue(item.GetValue(InObject)));
                        Console.Write($"@{SqlGenerators.GetName(item)}", property.GetValue(item.GetValue(InObject)));

                    }
                    else
                    {
                        command.Parameters.AddWithValue($"@{SqlGenerators.GetName(item)}", item.GetValue(InObject));
                        Console.Write($"@{SqlGenerators.GetName(item)}:{item.GetValue(InObject)} , ");
                    }

                    
                }
                Console.WriteLine();
                    
                
                command.ExecuteNonQuery();

                SqliteCommand command2 = new SqliteCommand($"SELECT last_insert_rowid()", conn);
                long insertedID = (long)command2.ExecuteScalar();
                
                IsWhatever.IsGetPrimaryKey(properties, out PropertyInfo? id2);
                id2?.SetValue(InObject, insertedID);
                Console.WriteLine($"$returned ID: {id2.GetValue(InObject)}");


            }
        }
        public static void Update(object InObject)
        {
            Type type = InObject.GetType();
            PropertyInfo[] properties = type.GetProperties();

            //welll i dont need it bcs if it sant id ill insert it
            //if (IsWhatever.IsGetPrimaryKey(properties, out PropertyInfo? id1) || id1 == null)
            //    throw new Exception("you want to update object which has no ID");

            //making querry to insert generic object
            StringBuilder sql = new StringBuilder();
            
            sql.Append($"UPDATE {type.Name} SET ");
            foreach (var item in properties)
            {
                if (IsWhatever.IsPrimaryKey(item))
                {
                    
                    continue;
                }
                if (IsWhatever.IsAttribute<DBForeignKeyAttribute>(item, out object itemType))
                {
                    if (IsWhatever.IsGetPrimaryKey(item.GetValue(InObject).GetType().GetProperties(), out PropertyInfo? id2) && id2.GetValue(item.GetValue(InObject)) != null)
                    {
                        sql.Append($"{SqlGenerators.GetName(item)} = @{SqlGenerators.GetName(item)}, ");
                    }
                    else
                    {
                        Insert(item.GetValue(InObject));
                        sql.Append($"{SqlGenerators.GetName(item)} = @{SqlGenerators.GetName(item)}, ");
                    }
                    continue;

                }

                if (IsWhatever.IsIgnore(item))
                {
                    continue;
                }

                sql.Append($"{SqlGenerators.GetName(item)} = @{SqlGenerators.GetName(item)}, ");

            }

            sql.Remove(sql.Length - 2, 2);
            
            if(IsWhatever.IsAttribute<DBPrimaryKeyAttribute>(properties, out List<PropertyInfo> propertyName)) {
                sql.Append($" WHERE {SqlGenerators.GetName(propertyName[0])} = @{SqlGenerators.GetName(propertyName[0])}");
            }
            else
            {
                throw new Exception("you want to update object which has no primary key collumn");
            }
            

            Console.WriteLine(sql);

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(sql.ToString(), conn);
                foreach (var item in properties)
                {
                    if (IsWhatever.IsForeignKey(item))
                    {
                        IsWhatever.IsGetPrimaryKey(item.GetValue(InObject).GetType().GetProperties(), out PropertyInfo? property);
                        
                        command.Parameters.AddWithValue($"@{SqlGenerators.GetName(item)}", property.GetValue(item.GetValue(InObject)));
                        Console.Write($"@{SqlGenerators.GetName(item)}:{property.GetValue(item.GetValue(InObject))} , ");
                    }
                    else
                    {
                        command.Parameters.AddWithValue($"@{SqlGenerators.GetName(item)}", (item.GetValue(InObject) == null) ? DBNull.Value : item.GetValue(InObject));
                        Console.Write($"@{SqlGenerators.GetName(item)}:{item.GetValue(InObject)} , ");
                    }

                }
                Console.WriteLine();

                command.ExecuteNonQuery();

            }
        }
        public static void InsertOrUpdate(object InObject)
        {
            Type type = InObject.GetType();
            PropertyInfo[] properties = type.GetProperties();
            //check if object has id
            if (IsWhatever.IsGetPrimaryKey(properties, out PropertyInfo? id1) && id1.GetValue(InObject) == null)
            {
                Insert(InObject);
            }
            else
            {
                Update(InObject);
            }
        }
        public static void Delete(object InObject)
        {
            Type type = InObject.GetType();
            PropertyInfo[] properties = type.GetProperties();


            //making querry to insert generic object
            StringBuilder sql = new StringBuilder();
            sql.Append($"DELETE FROM {type.Name} WHERE ");
            foreach (var item in properties)
            {
                if (IsWhatever.IsPrimaryKey(item))
                {
                    sql.Append($"{SqlGenerators.GetName(item)} = @{SqlGenerators.GetName(item)} AND ");
                }
            }
            sql.Remove(sql.Length - 5, 5);
            sql.Append(";");
            Console.WriteLine(sql);

            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(sql.ToString(), conn);
                foreach (var item in properties)
                {
                    if (IsWhatever.IsPrimaryKey(item))
                    {
                        command.Parameters.AddWithValue($"@{SqlGenerators.GetName(item)}", item.GetValue(InObject));
                        Console.Write($"@{SqlGenerators.GetName(item)}:{item.GetValue(InObject)}");
                    }
                }
                Console.WriteLine();

                command.ExecuteNonQuery();
            }
        }
        public static List<T> Select<T>(long id)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            //making querry to insert generic object
            StringBuilder sql = new StringBuilder();
            sql.Append($"SELECT ");
            
            foreach(var property in properties)
            {
                sql.Append($"{SqlGenerators.GetName(property)}, ");
            }
            sql.Remove(sql.Length - 2, 2);

            sql.Append($" FROM {type.Name} WHERE ");
            foreach (var item in properties)
            {
                if (IsWhatever.IsPrimaryKey(item))
                {
                    sql.Append($"{SqlGenerators.GetName(item)} = @{SqlGenerators.GetName(item)} AND ");
                }
            }
            sql.Remove(sql.Length - 5, 5);
            sql.Append(";");
            Console.WriteLine(sql);

            
            
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(sql.ToString(), conn);
                foreach (var item in properties)
                {
                    if (IsWhatever.IsPrimaryKey(item))
                    {
                        command.Parameters.AddWithValue($"@{SqlGenerators.GetName(item)}", id);
                        Console.Write($"@{SqlGenerators.GetName(item)}:{id}");
                    }
                    
                }
                Console.WriteLine();

                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                List<T> list = new List<T>();

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(Mapper.MapRow<T>(row));
                }

                return list;
            }

        }

        public static List<T> Select<T>(object foreignObject)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            Type typeForeign = foreignObject.GetType();

            IsWhatever.IsGetPrimaryKey(foreignObject.GetType().GetProperties(), out PropertyInfo idProp);
            Console.WriteLine(idProp.GetValue(foreignObject));
            long id = (long)idProp.GetValue(foreignObject);

            //making querry to insert generic object
            StringBuilder sql = new StringBuilder();
            sql.Append($"SELECT ");

            foreach (var property in properties)
            {
                sql.Append($"{SqlGenerators.GetName(property)}, ");
            }
            sql.Remove(sql.Length - 2, 2);

            sql.Append($" FROM {type.Name} WHERE ");
            foreach (var property in properties)
            {
                if (IsWhatever.IsForeignKey(property))
                {
                    foreach (var foreignProperty in typeForeign.GetProperties())
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{SqlGenerators.GetName(property)} = {SqlGenerators.GetName(foreignProperty)}");
                        Console.ResetColor();
                        if (SqlGenerators.GetName(foreignProperty) == SqlGenerators.GetName(property))
                        {
                            sql.Append($"{SqlGenerators.GetName(property)} = @{SqlGenerators.GetName(property)} AND ");
                        }
                        
                    }

                   
                    //sql.Append($"{SqlGenerators.GetName(item)} = @{SqlGenerators.GetName(item)} AND ");
                }
            }
            sql.Remove(sql.Length - 5, 5);
            sql.Append(";");
            //colors in console
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(sql);
            Console.ResetColor();



            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(sql.ToString(), conn);
                foreach (var property in properties)
                {

                    if (IsWhatever.IsForeignKey(property))
                    {
                        foreach (var foreignProperty in typeForeign.GetProperties())
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"{SqlGenerators.GetName(property)} = {SqlGenerators.GetName(foreignProperty)}");
                            Console.ResetColor();
                            if (SqlGenerators.GetName(foreignProperty) == SqlGenerators.GetName(property))
                            {
                                command.Parameters.AddWithValue($"@{SqlGenerators.GetName(property)}", id);
                                Console.Write($"@{SqlGenerators.GetName(property)}:{id}");
                                //sql.Append($"{SqlGenerators.GetName(property)} = @{SqlGenerators.GetName(property)} AND ");
                            }

                        }


                        //sql.Append($"{SqlGenerators.GetName(item)} = @{SqlGenerators.GetName(item)} AND ");
                    }


                }
                Console.WriteLine();

                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                List<T> list = new List<T>();

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(Mapper.MapRow<T>(row));
                }

                return list;
            }

        }

        public static List<T> Select<T>(Dictionary<string,string> keyValuePairs)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            //making querry to insert generic object
            StringBuilder sql = new StringBuilder();
            sql.Append($"SELECT ");

            foreach (var property in properties)
            {
                sql.Append($"{SqlGenerators.GetName(property)}, ");
            }
            sql.Remove(sql.Length - 2, 2);

            sql.Append($" FROM {type.Name} ");

            if (keyValuePairs != null || keyValuePairs?.Count > 0)
            {
                sql.Append($" WHERE ");

                foreach (var ittem in keyValuePairs)
                {
                    sql.Append($"{ittem.Key} = @{ittem.Key} AND ");
                }
                sql.Remove(sql.Length - 5, 5);
            }

            sql.Append(";");
            Console.WriteLine(sql);



            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                SqliteCommand command = new SqliteCommand(sql.ToString(), conn);

                if (keyValuePairs != null || keyValuePairs?.Count > 0)
                {
                    foreach (var item in keyValuePairs)
                    {
                        command.Parameters.AddWithValue($"@{item.Key}", item.Value);
                        Console.Write($"@{item.Key}:{item.Value}");
                    }
                }
                
                Console.WriteLine();

                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                List<T> list = new List<T>();

                foreach (DataRow row in dt.Rows)
                {
                    list.Add(Mapper.MapRow<T>(row));
                }

                return list;
            }

        }

        public static void DropDatabase()
        {
            File.Delete("database.db");
        }
        
        public static void DropTable<T>()
        {
            Type type = typeof(T);
            using (SqliteConnection conn = new SqliteConnection(connString))
            {
                conn.Open();
                string sql = $"DROP TABLE IF EXISTS {type.Name}";
                SqliteCommand command = new SqliteCommand(sql, conn);
                command.ExecuteNonQuery();
            }
        }
       

    }
}