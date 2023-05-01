using System;
using DataLayer.DatabaseEntites;
using DataLayer;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");

            DBConnection.DropDatabase();
            DBConnection.CreateDatabase();



            // Create a new user
            User user = new User()
            {
                
                UserName = "pepega1",
                Email = "idk@aa.cz",
                Password = "123456",
                Admin = false
            };



            // Create a new pool
            Place pool = new Place()
            {
                Type = "Pool",
                Price = 100,
                Capacity = 10,
                Description = "A pool with bubbles"
            };

            // Create a new sauna
            Place sauna = new Place()
            {
                Type = "Sauna",
                Price = 100,
                Capacity = 10,
                Humidity = 147

            };



            //Create reservation
            Reservation reservation = new Reservation()
            {
                Duration = 30,
                OverallPrice = 20,
                StartTime = DateTime.Now,
                Place = sauna,
                User = user,
                
                
            };





            // Save the pool to the database

            DBConnection.Insert(reservation);
            Console.WriteLine(user);
            DBConnection.Insert(new Reservation { Duration = 897, OverallPrice = 1, User = user, Place = sauna, StartTime = DateTime.Now });
            Console.WriteLine(user);
            reservation.Place = pool;
            reservation.OverallPrice = 2354678231456;
            DBConnection.Update(reservation);

            pool.Capacity = 321456;
            pool.Description = "hot tub stream pool";
            DBConnection.Update(pool);
            DBConnection.Update(new Place() { Capacity = 123456, Description = "hot tub stream pool" });
            reservation.Place = new Place { Type = "IDKNETU39M", Capacity = 2 };
            DBConnection.Update(reservation);

            var table = DBConnection.Select<Reservation>(1);
            Console.WriteLine("hen tu odsud je print");
            foreach (var row in table)
            {
                Console.WriteLine(row);
            }


            var user2 = new User()
            {
                UserName = "adminadmin",
                Email = "asdf@asdf",
                Password = "123456",
                Admin = true,

            };
            DBConnection.Insert(user2);
            var user3 = new User()
            {
                UserName = "nekdo jiny",
                Email = "asedfhjhjk@alksjdhfio",
                Password = "asdjklfjlsdka",
                Admin = true,

            };
            DBConnection.Insert(user3);

            var reervation2 = new Reservation()
            {
                Duration = 30,
                OverallPrice = 23,
                StartTime = DateTime.Now,
                Place = sauna,
                User = user2,


            };
            DBConnection.Insert(reervation2);
            var reervation3 = new Reservation()
            {
                Duration = 30,
                OverallPrice = 897564,
                StartTime = DateTime.Now,
                Place = sauna,
                User = user3,
            };
            DBConnection.Insert(reervation3);
            var reervation4 = new Reservation()
            {
                Duration = 30,
                OverallPrice = 347,
                StartTime = DateTime.Now,
                Place = sauna,
                User = user3,
            };
            DBConnection.Insert(reervation4);
            var reervation5 = new Reservation()
            {
                Duration = 30,
                OverallPrice = 951,
                StartTime = DateTime.Now,
                Place = sauna,
                User = user3,
            };
            DBConnection.Insert(reervation5);
            var reervation6 = new Reservation()
            {
                Duration = 30,
                OverallPrice = 710,
                StartTime = DateTime.Now,
                Place = sauna,
                User = user3,
            };
            DBConnection.Insert(reervation6);

            var table2 = DBConnection.Select<Reservation>(2);
            Console.WriteLine("hen tu odsud je print table 2");
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            foreach (var row in table2)
            {
                Console.WriteLine(row);
                Console.WriteLine("____________________________________________________________________________");
            }
            Console.WriteLine("000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            var table3 = DBConnection.Select<Reservation>(user3);
            Console.WriteLine("hen tu odsud je print table 3");
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            foreach (var row in table3)
            {
                Console.WriteLine(row);
                Console.WriteLine("____________________________________________________________________________");
            }





            Console.ReadKey();

        }
    }
}