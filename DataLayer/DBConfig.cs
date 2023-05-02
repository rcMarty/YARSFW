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
            string path = Directory.GetCurrentDirectory();

           
            Console.WriteLine("searching:" + Directory.GetFiles(path, "*.sln").Length);
            while (Directory.GetFiles(path, "*.sln").Length == 0)
            {
                //path = Directory.GetParent(path);
                Console.WriteLine("WHere am I?:" + path);
                path = Path.Combine(Directory.GetParent(path).FullName);
            }

            Console.WriteLine("Where is db source file: " + path + "Wellness.db");
            string executepath = Path.Combine(Directory.GetCurrentDirectory(), "Wellness.db");
            Console.WriteLine("Where will be DB copied" + executepath);
            //check if db exists
            if (File.Exists(executepath)) 
            {
                return executepath;
            }

            // copy db to the bin folder
            File.Copy(path, executepath, true);
            return executepath;

        }
    }
}
