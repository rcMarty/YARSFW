using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace DataLayer.DatabaseEntites
{
    public class Place
    {
        [DBPrimaryKey, DBName("Wellness_ID")]
        public long? ID { get; set; }
        [DBIsNullable]
        public double? Temperature { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
        [DBIsNullable]
        public string? Description { get; set; }

        public string Type { get; set; }

        //pool section

        [DBIsNullable]
        public int? Intensity { get; set; }
        [DBIsNullable]
        public int? Volume { get; set; }

        //sauna section
        [DBIsNullable]
        public double? Humidity { get; set; }


        //tostring
        public override string ToString()
        {
            return $"Place \n    " +
                    $"ID: {ID},\n      " +
                    $"Type: {Type}, Price: {Price}\n      " +
                    $"Temperature: {Temperature}, Capacity: {Capacity}, Intensity: {Intensity}, Volume: {Volume}, Humidity: {Humidity}, Description: {Description},";
        }

    }
}
