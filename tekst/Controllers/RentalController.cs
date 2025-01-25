using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tekst.Data;
using tekst.Models;

namespace tekst.Controllers
{
    [Authorize]
    public class RentalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalController(ApplicationDbContext context)
        {
            _context = context;
        }

     
        public IActionResult Index()
        {
            var rentals = _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .Select(r => new Rental
                {
                    Id = r.Id,
                    Car = new Car
                    {
                        Make = r.Car.Make,
                        Model = r.Car.Model
                    },
                    Customer = new Customer
                    {
                        FirstName = r.Customer.FirstName,
                        LastName = r.Customer.LastName
                    },
                    RentalDate = r.RentalDate,
                    ReturnDate = r.ReturnDate
                })
                .ToList();

            return View(rentals);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CustomerId,CarId,RentalDate,ReturnDate")] Rental rental)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(rental);
            }

            _context.Rentals.Add(rental);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental == null)
            {
                return NotFound();
            }

            PopulateDropdowns();
            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CustomerId,CarId,RentalDate,ReturnDate")] Rental rental)
        {
            if (id != rental.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(rental);
            }

            try
            {
                _context.Rentals.Update(rental);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rentals.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToAction("Index");
        }

        
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var rental = _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .FirstOrDefault(r => r.Id == id);
            if (rental == null) return NotFound();

            return View(rental);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var rental = _context.Rentals.Find(id);
            if (rental != null)
            {
                _context.Rentals.Remove(rental);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns()
        {
            ViewBag.Customers = new SelectList(
        _context.Customers.Select(c => new { c.Id, FullName = $"{c.FirstName} {c.LastName}" }),
        "Id",
        "FullName");

            ViewBag.Cars = new SelectList(
                _context.Cars.Select(c => new { c.Id, DisplayName = $"{c.Make} {c.Model}" }),
                "Id",
                "DisplayName");
        }
    }
}
