using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tekst.Data;
using tekst.Models;
using System.Linq;

namespace tekst.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            var cars = _context.Cars
                .Select(car => new Car
                {
                    Id = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    DailyPrice = car.DailyPrice,
                    IsAvailable = !_context.Rentals.Any(r => r.CarId == car.Id) 
                })
                .ToList();

            return View(cars);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            car.IsAvailable = car.IsAvailable == false;
            return View(car);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Cars.Update(car);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var car = _context.Cars.Find(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
