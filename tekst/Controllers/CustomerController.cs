using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tekst.Data;
using tekst.Models;
using System.Linq;

namespace tekst.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            var customers = _context.Customers
        .Select(customer => new Customer
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            HasRentedCar = _context.Rentals.Any(r => r.CustomerId == customer.Id)
        })
        .ToList();

            return View(customers);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = _context.Customers.Find(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Customer customer)
        {
            if (id != customer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(customer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
