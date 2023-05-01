using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseEntites
{
    public class Reservation
    {
        [DBPrimaryKey, DBName("Reservation_ID")]
        public long? ID { get; set; }
        
        public DateTime StartTime { get; set; }
        
        [DBForeignKey("User"), DBName("User_ID")]
        public User? User { get; set; }
         
        [DBForeignKey("Place"), DBName("Wellness_ID")]
        public Place? Place { get; set; }
       
        public int Duration { get; set; }
        
        public double OverallPrice { get; set; }

        public override string ToString()
        {
            return $"Reservation: \n" +
                   $"    ID: {ID}, StartTime: {StartTime} , Duration: {Duration}, OverallPrice: {OverallPrice} , \n" +
                   $"    User: \n{User}, \n" +
                   $"    Place: \n{Place}";
        }


    }
}
