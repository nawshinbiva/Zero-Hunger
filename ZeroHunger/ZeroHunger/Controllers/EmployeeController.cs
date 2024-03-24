using System;
using System.Linq;
using System.Web.Mvc;
using ZeroHunger.DTO;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class EmployeeController : Controller
    {
        private ZeroHungerEntities db = new ZeroHungerEntities();

        // GET: Employee/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Employee/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(EmployeeDTO employeeDTO)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Employee_Name = employeeDTO.Employee_Name,
                    Contact_Number = employeeDTO.Contact_Number,
                    Username = employeeDTO.Username,
                    Password = employeeDTO.Password
                };

                db.Employees.Add(employee);
                db.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(employeeDTO);
        }

        // GET: Employee/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Employee/Login
        [HttpPost]
        
        public ActionResult Login(EmployeeDTO employeeDTO)
        {
            
                var employee = db.Employees.FirstOrDefault(e => e.Username.Trim() == employeeDTO.Username.Trim() && e.Password == employeeDTO.Password);

                if (employee != null)
                {
                    Session["Employee_ID"] = employee.Employee_ID;

                    return RedirectToAction("Dashboard", "Employee");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View(employeeDTO);
                }
            

            
        }

        // GET: Employee/Logout
        public ActionResult Logout()
        {
            // Clear the session
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // Helper method to get the ID of the currently logged in employee
        private int GetCurrentEmployeeId()
        {
            // Check if the employee ID is stored in the session
            if (Session["Employee_ID"] != null && Session["Employee_ID"] is int)
            {
                return (int)Session["Employee_ID"];
            }
            else
            {
                return -1;
            }
        }

        // GET: Employee/Dashboard
        public ActionResult Dashboard()
        {
            int employeeId = GetCurrentEmployeeId();

            var assignedRequestsWithDetails = db.CollectAssignments
                .Where(ca => ca.Employee_ID == employeeId)
                .Join(db.CollectRequests,
                      ca => ca.Request_ID,
                      cr => cr.Request_ID,
                      (ca, cr) => new AssignedRequestDTO
                      {
                          Request_ID = cr.Request_ID,
                          Restaurant_ID = cr.Restaurant_ID,
                          Request_DateTime = cr.Request_DateTime,
                          Maximum_Preservation_Time = cr.Maximum_Preservation_Time,
                          FoodItem_Name = cr.FoodItem_Name,
                          FoodItem_Quantity = cr.FoodItem_Quantity,
                          
                      })
                .ToList();

            return View(assignedRequestsWithDetails);
        }

        // POST: Employee/CompleteRequest
        [HttpPost]
        public ActionResult CompleteRequest(int requestId)
        {
            // Retrieve the collect request from the database
            var collectRequest = db.CollectRequests.FirstOrDefault(c => c.Request_ID == requestId);

            if (collectRequest != null)
            {
                // Update the status to "Completed"
                collectRequest.Status = "Completed";
                // Save changes to the database
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                // If the collect request is not found, return an error
                return Json(new { success = false });
            }
        }
    }
}
