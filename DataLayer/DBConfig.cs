using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataLayer
{
    public static class DBConfig
    {
        private static string? _connectionString = null;
        public static string ConnectionString { 
            get 
            {
                if (_connectionString == null)
                {
                    return CopyDB();
                }
                
                return _connectionString; 
            } 
            set 
            {
                _connectionString = value;
            } 
        }

        private static string CopyDB()
        {
            //get path to the db which is in folder assets where .sln of this project is
            string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "assets", "Wellness.db");
            Console.WriteLine(path);
            string executepath = Path.Combine(Directory.GetCurrentDirectory(), "Wellness.db");
            //check if db exists
            if (File.Exists(executepath)) 
            {
                Console.WriteLine(executepath);
                return executepath;
            }

            // copy db to the bin folder
            File.Copy(path, executepath, true);
            return executepath;

        }
    }
}
