using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DatabaseEntites
{
    public class User
    {
        [DBPrimaryKey, DBName("User_ID")]
        public long? ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        [DBIsNullable]
        public string? FirstName { get; set; }
        [DBIsNullable]
        public string? LastName { get; set; }
        public bool Admin { get; set; }

        public override string ToString()
        {
            return $"User: \n      " +
                   $"   ID: {ID}\n" +
                   $"   UserName: {UserName}, Password: {Password}, Email: {Email} \n" +
                   $"   FirstName: {FirstName}, LastName: {LastName}, Admin: {Admin}";
        }


    }
}
