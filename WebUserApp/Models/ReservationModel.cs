using DataLayer.DatabaseEntites;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace WebUserApp.Models
{
    public class ReservationModel
    {
        [ValidateNever]
        public Place Place { get; set; }
        public long PlaceID { get; set; }
        [Required(ErrorMessage = "Duration is requried!"), IntegerValidator]
        public int Duration { get; set; }
        [Required(ErrorMessage = "Start time is requried!")]
        [MyDate(ErrorMessage = "Invalid date")]
        public DateTime StartTime { get; set; }
        
        
    }

    public class MyDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)// Return a boolean value: true == IsValid, false != IsValid
        {
            DateTime d = Convert.ToDateTime(value);
            return d >= DateTime.Now; //Dates Greater than or equal to today are valid (true)

        }
    }
}
