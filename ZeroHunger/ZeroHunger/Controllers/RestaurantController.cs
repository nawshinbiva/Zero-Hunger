using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTO;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class RestaurantController : Controller
    {

        private ZeroHungerEntities db = new ZeroHungerEntities();

        // GET: Restaurant/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Restaurant/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RestaurantDTO restaurantDTO)
        {
            if (ModelState.IsValid)
            {
                var restaurant = new Restaurant
                {
                    Restaurant_Name = restaurantDTO.Restaurant_Name,
                    Contact_Number = restaurantDTO.Contact_Number,
                    Location = restaurantDTO.Location,
                    Email = restaurantDTO.Email,
                    Password = restaurantDTO.Password
                };

                db.Restaurants.Add(restaurant);
                db.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(restaurantDTO);
        }


        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Login(RestaurantDTO restaurantDTO)
        {
            
                var restaurant = db.Restaurants.FirstOrDefault(r => r.Email == restaurantDTO.Email && r.Password == restaurantDTO.Password);

                if (restaurant != null)
                {
                    Session["Restaurant_ID"] = restaurant.Restaurant_ID;

                    return RedirectToAction("Dashboard", "Restaurant");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(restaurantDTO);
                }
            

            
        }

        // GET: Restaurant/Logout
        public ActionResult Logout()
        {
            Session.Clear(); 
            return RedirectToAction("Index", "Home"); 
        }

        // Helper method to get the ID of the currently logged in restaurant
        private int GetCurrentRestaurantId()
        {
            // Check if the restaurant ID is stored in the session
            if (Session["Restaurant_ID"] != null && Session["Restaurant_ID"] is int)
            {
                return (int)Session["Restaurant_ID"];
            }
            else
            {
                return -1;

            }
        }

        // GET: Restaurant/Dashboard
        public ActionResult Dashboard()
        {
            // Retrieve collect requests for the current restaurant
            int currentRestaurantId = GetCurrentRestaurantId(); // Implement this method to get the current restaurant ID
            var collectRequests = db.CollectRequests
                                     .Where(r => r.Restaurant_ID == currentRestaurantId)
                                     .ToList();

            // Convert CollectRequest entities to CollectRequestDTO objects
            var collectRequestDTOs = collectRequests.Select(r => new CollectRequestDTO
            {
                Request_ID = r.Request_ID,
                Restaurant_ID = r.Restaurant_ID,
                Request_DateTime = r.Request_DateTime,
                Maximum_Preservation_Time = r.Maximum_Preservation_Time,
                FoodItem_Name = r.FoodItem_Name,
                FoodItem_Quantity = r.FoodItem_Quantity,
                Status = r.Status
            });

            return View(collectRequestDTOs);
        }

        // GET: Restaurant/CreateCollectRequest
        public ActionResult CreateCollectRequest()
        {
            // Initialize a new instance of CollectRequestDTO
            var collectRequestDTO = new CollectRequestDTO
            {
                // Set default values
                Request_DateTime = DateTime.Now,
                Status = "Pending"
            };

            return View(collectRequestDTO);
        }

        // POST: Restaurant/CreateCollectRequest
        [HttpPost]

        public ActionResult CreateCollectRequest(CollectRequestDTO collectRequestDTO)
        {
            try
            {
                int currentRestaurantId = GetCurrentRestaurantId();
                if (currentRestaurantId == -1)
                {
                    // Redirect to login if not logged in
                    return RedirectToAction("Login");
                }

                var collectRequestEntity = new CollectRequest
                {
                    Request_DateTime = DateTime.Now,
                    Restaurant_ID = currentRestaurantId,
                    Maximum_Preservation_Time = collectRequestDTO.Maximum_Preservation_Time,
                    FoodItem_Name = collectRequestDTO.FoodItem_Name,
                    FoodItem_Quantity = collectRequestDTO.FoodItem_Quantity,
                    Status = "Pending"
                };

                db.CollectRequests.Add(collectRequestEntity);
                db.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error");
            }
        }





    }
}