using Cargo_FinalApplication.Models;
using Cargo_FinalApplication.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Cargo_FinalApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseInventoryController : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public WarehouseInventoryController(FinalCargoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetWarehouseInventories()
        {
            var warehouseInventories = _context.WarehouseInventories
                .Include(wi => wi.Cargo)
                .Include(wi => wi.Warehouse)
                .ToList();

            return Ok(warehouseInventories);
        }

        [HttpGet("{warehouseId}/{cargoId}")]
        public IActionResult GetWarehouseInventory(int warehouseId, int cargoId)
        {
            var warehouseInventory = _context.WarehouseInventories
                .Include(wi => wi.Cargo)
                .Include(wi => wi.Warehouse)
                .FirstOrDefault(wi => wi.WarehouseId == warehouseId && wi.CargoId == cargoId);

            if (warehouseInventory == null)
            {
                return NotFound();
            }

            return Ok(warehouseInventory);
        }

        // POST: api/WarehouseInventory
        [HttpPost]
        public IActionResult CreateWarehouseInventory( Warehousedetails warehouseInventory)
        {
            var warehouse = new WarehouseInventory()
            {
                WarehouseId=warehouseInventory.WarehouseId, 
                CargoId=warehouseInventory.CargoId,
                Quantity=warehouseInventory.Quantity
            };
            _context.WarehouseInventories.Add(warehouse);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{warehouseId}/{cargoId}")]
        public IActionResult DeleteWarehouseInventory(int warehouseId, int cargoId)
        {
            var warehouseInventory = _context.WarehouseInventories
                .FirstOrDefault(wi => wi.WarehouseId == warehouseId && wi.CargoId == cargoId);

            if (warehouseInventory == null)
            {
                return NotFound();
            }

            _context.WarehouseInventories.Remove(warehouseInventory);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
