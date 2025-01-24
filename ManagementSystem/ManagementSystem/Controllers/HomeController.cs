using ManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly InstituteContext context;

        public HomeController(InstituteContext context)
        {
            this.context = context;
        }

        // GET: Login Page
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        // POST: Handle Login
        [HttpPost]
        public IActionResult Login(Register register)
        {
            var myUser = context.Registers
                .FirstOrDefault(x => x.Email == register.Email && x.Password == register.Password);

            if (myUser != null)
            {
                // Set session values for the logged-in user
                HttpContext.Session.SetString("UserSession", myUser.Email);
                HttpContext.Session.SetString("UserFullName", $"{myUser.Name}");
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Login Failed. Please check your credentials.";
                return View();
            }
        }

        // Logout Action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears all session data
            return RedirectToAction("Login");
        }

        // Dashboard Action
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") == null)
            {
                return RedirectToAction("Login");
            }

            // Fetch dashboard statistics
            ViewData["TotalInquiry"] = context.Inquiries.Count();
            ViewData["TotalAdmission"] = context.Students.Count();
            ViewData["TotalEmployee"] = context.Employees.Count();
            ViewData["TotalCourse"] = context.Courses.Count();
            ViewData["TotalCollection"] = context.Students.Sum(a => a.ReceiveAmount);
            ViewData["TotalPending"] = context.Students.Sum(a => a.PendingAmount);

            // Set the user's full name in the ViewBag for display
            ViewBag.FullName = HttpContext.Session.GetString("UserFullName");
            ViewBag.Mysession = HttpContext.Session.GetString("UserSession");

            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
