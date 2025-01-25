using Microsoft.AspNetCore.Mvc;
using tekst.Data;
using tekst.Models;

namespace tekst.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAvailableCars()
        {
            var cars = _context.Cars
                .Select(c => new
                {
                    c.Id,
                    Name = $"{c.Make} {c.Model}"
                })
                .ToList();

            return Ok(cars);
        }
    }
}
