using Microsoft.AspNetCore.Mvc;
using DataLayer;
using DataLayer.DatabaseEntites;
using WebUserApp.Models;

namespace WebUserApp.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            List<Place>allplaces = DBConnection.Select<Place>(null);
            List<string> types = new List<string>();
            foreach (Place place in allplaces)
            {
                if(types.Contains(place.Type))
                    continue;

                types.Add(place.Type.ToString());
            }

            ViewBag.types = types;
            ViewBag.allplaces = allplaces;


            return View();
        }

        public IActionResult YourReservations()
        {
            if (UserSingleton.Instance.loggedUser == null)
                return RedirectToAction("Index", "Home");
            
            List<Reservation> reservations = DBConnection.Select<Reservation>(UserSingleton.Instance.loggedUser);

            ViewBag.reservations = reservations;
            
            return View();
        }   

        

      

        public IActionResult Cancel(long id)
        {
            if (UserSingleton.Instance.loggedUser == null)
                return RedirectToAction("Index", "Home");
            
            List<Reservation> reservations = DBConnection.Select<Reservation>(id);
            if (reservations.Count == 0)
                return RedirectToAction("Index", "Home");
            Reservation reservation = reservations[0];
            if (reservation.User.ID != UserSingleton.Instance.loggedUser.ID)
                return RedirectToAction("Index", "Home");
            DBConnection.Delete(reservation);
            return RedirectToAction("YourReservations", "Reservation");
        }


        public IActionResult Reserve(long id)
        {
            if (UserSingleton.Instance.loggedUser == null)
                return RedirectToAction("Index", "Home");
            List<Place> place = DBConnection.Select<Place>(id);
            if (place == null)
                return RedirectToAction("Index", "Home");

            ViewBag.place = place[0];
            return View();
        }


        [HttpPost]
        public IActionResult NewReservation(ReservationModel model)
        {
            Place place = DBConnection.Select<Place>(model.PlaceID)[0];
            ViewBag.place = place;
            
            model.Place = place;

            if (UserSingleton.Instance.loggedUser == null)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View("Reserve", model);

            List<Reservation> capacity = DBConnection.Select<Reservation>(place);

            //long availablePlaces = place.Capacity - capacity.Count;
            //if (availablePlaces <= 0)
                //return RedirectToAction("Index", "Home");
            

            Reservation reservation = new Reservation()
            {
                StartTime = model.StartTime,
                Place = place,
                User = UserSingleton.Instance.loggedUser,
                Duration = model.Duration,
                OverallPrice = (place.Price / 0.247) * model.Duration
               
            };

            if (CheckFreePlace(reservation))
            {
                DBConnection.Insert(reservation);
                return RedirectToAction("YourReservations", "Reservation");
            }
            else
            {   
                ModelState.AddModelError("StartTime", "This place is already FULL in this time");
                return View("Reserve", model);
            }

        }

        private bool CheckFreePlace(Reservation newres)
        {
            //chceck if there is collision with other reservations for the same place in the same time 
            List<Reservation> current = DBConnection.Select<Reservation>(newres.Place);

            
            if(current.Count <= newres.Place.Capacity)
                return true;

            foreach (Reservation reservation in current)
            {
                if (reservation.StartTime == newres.StartTime)
                    return false;
                if (reservation.StartTime < newres.StartTime && reservation.StartTime.AddMinutes(reservation.Duration) > newres.StartTime)
                    return false;
                if (reservation.StartTime > newres.StartTime && reservation.StartTime < newres.StartTime.AddMinutes(newres.Duration))
                    return false;

            }
            return true;
        }
    }
}
