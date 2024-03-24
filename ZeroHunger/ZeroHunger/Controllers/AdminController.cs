using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZeroHunger.DTO;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class AdminController : Controller
    {
        private ZeroHungerEntities db = new ZeroHungerEntities();

        // GET: Admin Dashboard
        public ActionResult Dashboard()
        {
            var collectRequests = db.CollectRequests.ToList();
            var collectRequestDTOs = collectRequests.Select(cr => new CollectRequestDTO
            {
                Request_ID = cr.Request_ID,
                Restaurant_ID = cr.Restaurant_ID,
                Request_DateTime = cr.Request_DateTime,
                Maximum_Preservation_Time = cr.Maximum_Preservation_Time,
                FoodItem_Name = cr.FoodItem_Name,
                FoodItem_Quantity = cr.FoodItem_Quantity,
                Status = cr.Status
            }).ToList();

            return View(collectRequestDTOs);
        }

        // GET: Review Pending Collect Requests
        // GET: Review Pending Collect Requests
        public ActionResult ReviewCollectRequests()
        {
            var pendingCollectRequests = db.CollectRequests.Where(c => c.Status == "Pending").ToList();
            ViewBag.Employees = db.Employees.ToList(); // Get the list of employees from the database
            return View(pendingCollectRequests);
        }



        // POST: Approve Collect Request
        [HttpPost]
        public ActionResult Approve(int collectRequestId)
        {
            var collectRequest = db.CollectRequests.FirstOrDefault(c => c.Request_ID == collectRequestId);

            if (collectRequest != null)
            {
                collectRequest.Status = "Approved";
                db.SaveChanges();
            }

            return RedirectToAction("ReviewCollectRequests");
        }


        // POST: Assign Collect Request
        [HttpPost]
        public ActionResult Assign(int collectRequestId, int employeeId)
        {
            try
            {
                var collectRequest = db.CollectRequests.FirstOrDefault(c => c.Request_ID == collectRequestId);

                if (collectRequest != null)
                {
                    // Check if the provided employeeId exists
                    var employee = db.Employees.FirstOrDefault(e => e.Employee_ID == employeeId);
                    if (employee == null)
                    {
                        ModelState.AddModelError("", "Employee not found.");
                        var pendingCollectRequests = db.CollectRequests.Where(c => c.Status == "Pending").ToList();
                        ViewBag.Employees = db.Employees.ToList(); // Update ViewBag with employees again
                        return View("ReviewCollectRequests", pendingCollectRequests);
                    }

                    // Create a new collect assignment
                    var collectAssignment = new CollectAssignment
                    {
                        Request_ID = collectRequest.Request_ID,
                        Employee_ID = employeeId,
                        Assignment_DateTime = DateTime.Now,
                        Status = "Assigned"
                    };

                    // Add collect assignment and update collect request status
                    db.CollectAssignments.Add(collectAssignment);
                    collectRequest.Status = "Assigned";
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Collect request not found.");
                    var pendingCollectRequests = db.CollectRequests.Where(c => c.Status == "Pending").ToList();
                    ViewBag.Employees = db.Employees.ToList(); // Update ViewBag with employees again
                    return View("ReviewCollectRequests", pendingCollectRequests);
                }

                // Redirect back to the ReviewCollectRequests action
                return RedirectToAction("ReviewCollectRequests");
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError("", "An error occurred while processing the request.");
                var pendingCollectRequests = db.CollectRequests.Where(c => c.Status == "Pending").ToList();
                ViewBag.Employees = db.Employees.ToList(); // Update ViewBag with employees again
                return View("ReviewCollectRequests", pendingCollectRequests);
            }
        }

    }
}
