using Cargo_FinalApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Final_CargoApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public CargoController(FinalCargoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCargo()
        {
            var cargos = _context.Cargos.Include(c => c.Warehouse).ToList();
            return Ok(cargos);
        }

     
        [HttpGet("{id}")]
        public IActionResult GetCargo(int id)
        {
            var cargo = _context.Cargos.Include(c => c.Warehouse).FirstOrDefault(c => c.CargoId == id);

            if (cargo == null)
            {
                return NotFound("Cargo not found");
            }

            return Ok(cargo);
        }

        [HttpPost]
        public IActionResult PostCargo(Cargo cargo)
        {
            _context.Cargos.Add(cargo);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCargo), new { id = cargo.CargoId }, cargo);
        }

  
        [HttpPut("{id}")]
        public IActionResult PutCargo(int id, Cargo cargo)
        {
            if (id != cargo.CargoId)
            {
                return BadRequest("Cargo ID mismatch");
            }

            _context.Entry(cargo).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

     
        [HttpDelete("{id}")]
        public IActionResult DeleteCargo(int id)
        {
            var cargo = _context.Cargos.Find(id);
            if (cargo == null)
            {
                return NotFound("Cargo not found");
            }

            _context.Cargos.Remove(cargo);
            _context.SaveChanges();

            return NoContent();
        }

        
        [HttpGet("check-weight/{cargoId}")]
        public IActionResult CheckWeight(int cargoId)
        {
            var cargo = _context.Cargos.Find(cargoId);
            if (cargo == null)
            {
                return NotFound("Cargo not found");
            }

            return Ok(new { Weight = cargo.Weight });
        }

       
        [HttpGet("check-damage/{cargoId}")]
        public IActionResult CheckDamage(int cargoId)
        {
            var cargo = _context.Cargos.Find(cargoId);
            if (cargo == null)
            {
                return NotFound("Cargo not found");
            }

            return Ok(new { IsDamaged = cargo.Damaged });
        }

      
        [HttpPost("reorder/{cargoId}")]
        public IActionResult Reorder(int cargoId)
        {
            var cargo = _context.Cargos.Find(cargoId);
            if (cargo == null)
            {
                return NotFound("Cargo not found");
            }

            if (!cargo.Damaged)
            {
                return BadRequest("Cargo is not damaged");
            }

            var newOrder = new Cargo
            {
                CargoName = cargo.CargoName,
                Weight = cargo.Weight,
                Damaged = false,
                DateStored = DateTime.Now,
                WarehouseId = cargo.WarehouseId
            };

            _context.Cargos.Add(newOrder);
            _context.SaveChanges();

            return Ok(new { Message = "Reorder placed successfully", NewOrderId = newOrder.CargoId });
        }

       
        [HttpPost("dispatch")]
        public IActionResult DispatchCargo(int cargoId, int warehouseId)
        {
            try
            {
              
                var cargo = _context.Cargos.Find(cargoId);
                if (cargo == null)
                {
                    return NotFound("Cargo not found");
                }

              
                var warehouse = _context.Warehouses.Include(w => w.Cargos).FirstOrDefault(w => w.WarehouseId == warehouseId);
                if (warehouse == null)
                {
                    return NotFound("Warehouse not found");
                }

               
                if (cargo.WarehouseId == warehouseId)
                {
                    return BadRequest("Cargo is already in the specified warehouse");
                }

              
                if (cargo.Warehouse != null)
                {
                    var currentWarehouse = _context.Warehouses.Include(w => w.Cargos).FirstOrDefault(w => w.WarehouseId == cargo.WarehouseId);
                    if (currentWarehouse != null)
                    {
                        currentWarehouse.Cargos.Remove(cargo);
                    }
                }
               
                warehouse.Cargos.Add(cargo);

                
                cargo.WarehouseId = warehouseId;

                
                var gatePass = new GatePass
                {
                    OrderId = cargoId, 
                    DispatchDate = DateTime.Now,
                    CreatedBy = 1,
                };
                _context.GatePasses.Add(gatePass);

                _context.SaveChanges();

                return Ok(new { Message = "Cargo dispatched successfully", CargoId = cargo.CargoId, GatePassId = gatePass.GatePassId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
        [HttpPost("shift")]
        public IActionResult ShiftCargo(int cargoId, int fromWarehouseId, int toWarehouseId)
        {
            try
            {
               
                var cargo = _context.Cargos.Find(cargoId);
                if (cargo == null)
                {
                    return NotFound("Cargo not found");
                }

            
                var fromWarehouse = _context.Warehouses.Include(w => w.Cargos).FirstOrDefault(w => w.WarehouseId == fromWarehouseId);
                var toWarehouse = _context.Warehouses.Include(w => w.Cargos).FirstOrDefault(w => w.WarehouseId == toWarehouseId);

                if (fromWarehouse == null)
                {
                    return NotFound("Source warehouse not found");
                }

                if (toWarehouse == null)
                {
                    return NotFound("Target warehouse not found");
                }

              
                var order = _context.Orders.FirstOrDefault(o => o.FromWarehouseId == fromWarehouseId && o.ToWarehouseId == toWarehouseId);

                if (order == null)
                {
                  
                    order = new Order
                    {
                        CargoName = cargo.CargoName,
                        Quantity = (int)cargo.Weight, 
                        FromWarehouseId = fromWarehouseId,
                        ToWarehouseId = toWarehouseId,
                        OrderDate = DateTime.Now,
                        Status = "Pending",
                        IsOutgoing = true 
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();
                }

                
                cargo.WarehouseId = toWarehouseId;
                _context.SaveChanges();

              
                var gatePass = new GatePass
                {
                    OrderId = order.OrderId,
                    DispatchDate = DateTime.Now,
                    CreatedBy = 1 
                };
                _context.GatePasses.Add(gatePass);
                _context.SaveChanges();

                return Ok(new { Message = "Cargo shifted successfully", CargoId = cargo.CargoId, GatePassId = gatePass.GatePassId });
            }
            catch (DbUpdateException ex)
            {
               
                return StatusCode(500, $"Internal server error: {ex.Message}, {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    
        [HttpGet("track/{cargoId}")]
        public IActionResult TrackCargo(int cargoId)
        {
            try
            { 
                var cargo = _context.Cargos.Include(c => c.Warehouse).FirstOrDefault(c => c.CargoId == cargoId);

                if (cargo == null)
                {
                    return NotFound("Cargo not found");
                }

              
                var warehouseDetails = new
                {
                    WarehouseId = cargo.WarehouseId,
                    WarehouseName = cargo.Warehouse?.WarehouseName,
                    Location = cargo.Warehouse?.Location
                };

                return Ok(new { CargoId = cargo.CargoId, CargoName = cargo.CargoName, Warehouse = warehouseDetails });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
