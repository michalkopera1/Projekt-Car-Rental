using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tekst.Data;
using tekst.Models;

namespace tekst.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            
            var totalCars = _context.Cars.Count();
            var rentedCars = _context.Rentals.Select(r => r.CarId).Distinct().Count();

            var totalCustomers = _context.Customers.Count();
            var activeRentals = _context.Rentals.Select(r => r.CustomerId).Distinct().Count();

            
            ViewData["TotalCars"] = totalCars;
            ViewData["RentedCars"] = rentedCars;
            ViewData["TotalCustomers"] = totalCustomers;
            ViewData["ActiveRentals"] = activeRentals;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
