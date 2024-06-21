using Cargo_FinalApplication.Models;
using Cargo_FinalApplication.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CargoBookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatePassesController : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public GatePassesController(FinalCargoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetGatePasses()
        {
            var gatePasses = _context.GatePasses.ToList();
            return Ok(gatePasses);
        }

        [HttpGet("{id}")]
        public IActionResult GetGatePass(int id)
        {
            var gatePass = _context.GatePasses.Find(id);
            if (gatePass == null)
            {
                return NotFound();
            }
            return Ok(gatePass);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult PostGatePass(GatePassDto gatePassDto)
        {
            var gatePass = new GatePass
            {
                OrderId = gatePassDto.OrderId,
                DispatchDate = gatePassDto.DispatchDate,
                CreatedBy = gatePassDto.CreatedBy
            };

            _context.GatePasses.Add(gatePass);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetGatePass), new { id = gatePass.GatePassId }, gatePass);
        }


        [HttpPut("{id}")]
        public IActionResult PutGatePass(int id, GatePassDto gatePassDto)
        {
            if (id != gatePassDto.GatePassId)
            {
                return BadRequest();
            }

            var gatePass = new GatePass
            {
                GatePassId = gatePassDto.GatePassId,
                OrderId = gatePassDto.OrderId,
                DispatchDate = gatePassDto.DispatchDate,
                CreatedBy = gatePassDto.CreatedBy
            };

            _context.Entry(gatePass).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GatePassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteGatePass(int id)
        {
            var gatePass = _context.GatePasses.Find(id);
            if (gatePass == null)
            {
                return NotFound();
            }

            _context.GatePasses.Remove(gatePass);
            _context.SaveChanges();

            return NoContent();
        }

        private bool GatePassExists(int id)
        {
            return _context.GatePasses.Any(e => e.GatePassId == id);
        }

    }
}
