using Cargo_FinalApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Cargo_FinalApplication.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Final_CargoApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public WarehousesController(FinalCargoDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public IActionResult GetWarehouses()
        {
            var warehouses = _context.Warehouses.ToList();
            return Ok(warehouses);
        }

       
        [HttpGet("{id}")]
        public IActionResult GetWarehouse(int id)
        {
            var warehouse = _context.Warehouses.Find(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return Ok(warehouse);
        }

       
        [HttpPost]
        public IActionResult PostWarehouse(WarehouseDto warehouseDto )
        {
            var warehouse = new Warehouse
            {
                WarehouseName=warehouseDto.WarehouseName,
                Capacity = warehouseDto.Capacity,   
                Location=warehouseDto.Location,
                MobileNum=warehouseDto.MobileNum,
            };
           
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.WarehouseId }, warehouse);
        }

        [HttpPut("{id}")]
        public IActionResult PutWarehouse(int id, Warehouse warehouse)
        {
            if (id != warehouse.WarehouseId)
            {
                return BadRequest();
            }

            var existingWarehouse = _context.Warehouses.Find(id);

            if (existingWarehouse == null)
            {
                return NotFound();
            }

            existingWarehouse.WarehouseName = warehouse.WarehouseName;
            existingWarehouse.Location = warehouse.Location;
            existingWarehouse.Capacity = warehouse.Capacity;
            existingWarehouse.MobileNum = warehouse.MobileNum;

            _context.Entry(existingWarehouse).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WarehouseExists(id))
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
        public IActionResult DeleteWarehouse(int id)
        {
            var warehouse = _context.Warehouses.Find(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            _context.Warehouses.Remove(warehouse);
            _context.SaveChanges();

            return NoContent();
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.WarehouseId == id);
        }
    }
}
