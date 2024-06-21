using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Cargo_FinalApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CargoBookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public ReportsController(FinalCargoDbContext context)
        {
            _context = context;
        }

        // GET: api/reports/orders-by-status
        [HttpGet("orders-by-status")]
        public IActionResult GetOrdersByStatus()
        {
            var orderCounts = _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            return Ok(orderCounts);
        }
    }
}
