using Cargo_FinalApplication.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Final_CargoApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FinalCargoDbContext _context;

        public OrdersController(FinalCargoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            var orders = _context.Orders
                                  .Include(o => o.Sender)
                                  .Include(o => o.Receiver)
                                  .Include(o => o.FromWarehouse)
                                  .Include(o => o.ToWarehouse)
                                  .ToList();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.Orders
                                  .Include(o => o.Sender)
                                  .Include(o => o.Receiver)
                                  .Include(o => o.FromWarehouse)
                                  .Include(o => o.ToWarehouse)
                                  .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        [HttpPost]
        public IActionResult PostOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }


        [HttpPut("{id}")]
        public IActionResult PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("TotalOrders")]
        public IActionResult GetTotalOrders()
        {
            var totalOrders = _context.Orders.Count();
            return Ok(totalOrders);
        }


        [HttpGet("PendingOrders")]
        public IActionResult GetPendingOrders()
        {
            var pendingOrders = _context.Orders.Count(o => o.Status == "Pending");
            return Ok(pendingOrders);
        }


        [HttpGet("CompletedOrders")]
        public IActionResult GetCompletedOrders()
        {
            var completedOrders = _context.Orders.Count(o => o.Status == "Delivered");
            return Ok(completedOrders);
        }


        [HttpGet("CancelledOrders")]
        public IActionResult GetCancelledOrders()
        {
            var cancelledOrders = _context.Orders.Count(o => o.Status == "Cancelled");
            return Ok(cancelledOrders);
        }

        [HttpPost("send")]
        public IActionResult SendOrder(Order order)
        {

            order.OrderDate = DateTime.UtcNow;


            _context.Orders.Add(order);
            _context.SaveChanges();


            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }


        [HttpGet("receive/{userId}")]
        public IActionResult GetReceivedOrders(int userId)
        {
            var receivedOrders = _context.Orders
                .Where(o => o.ReceiverId == userId)
                .Include(o => o.Sender)
                .ToList();

            return Ok(receivedOrders);
        }


        [HttpGet("OrdersGroupedByDate")]
        public IActionResult GetOrdersGroupedByDate()
        {
            var orders = _context.Orders
                                 .GroupBy(o => o.OrderDate.Date)
                                 .Select(g => new OrdersByDate
                                 {
                                     Date = g.Key,
                                     Count = g.Count()
                                 })
                                 .ToList();

            if (orders == null || !orders.Any())
            {
                return NotFound("No orders found.");
            }

            return Ok(orders);
        }

        [HttpGet("DownloadReport")]
        public IActionResult DownloadReport()
        {
            if (_context == null)
            {
                return StatusCode(500, "Database context is not available.");
            }

            var orders = _context.Orders
                .Include(o => o.Sender)
                .Include(o => o.Receiver)
                .Include(o => o.FromWarehouse)
                .Include(o => o.ToWarehouse)
                .ToList();

            var warehouses = _context.Warehouses
                .Include(w => w.WarehouseInventories)
                .ThenInclude(inv => inv.Cargo)
                .ToList();

            var cargos = _context.Cargos
                .Include(c => c.Warehouse)
                .ToList();

          
            if (orders == null || warehouses == null || cargos == null)
            {
                return StatusCode(500, "Failed to retrieve data from the database.");
            }

            using (var workbook = new XLWorkbook())
            {
                var ordersSheet = workbook.Worksheets.Add("Orders");
                var warehousesSheet = workbook.Worksheets.Add("Warehouses");
                var cargosSheet = workbook.Worksheets.Add("Cargos");

                ordersSheet.Cell(1, 1).Value = "OrderId";
                ordersSheet.Cell(1, 2).Value = "Sender";
                ordersSheet.Cell(1, 3).Value = "Receiver";
                ordersSheet.Cell(1, 4).Value = "CargoName";
                ordersSheet.Cell(1, 5).Value = "Quantity";
                ordersSheet.Cell(1, 6).Value = "FromWarehouse";
                ordersSheet.Cell(1, 7).Value = "ToWarehouse";
                ordersSheet.Cell(1, 8).Value = "OrderDate";
                ordersSheet.Cell(1, 9).Value = "Status";

                for (int i = 0; i < orders.Count; i++)
                {
                    var order = orders[i];
                    ordersSheet.Cell(i + 2, 1).Value = order.OrderId;
                    ordersSheet.Cell(i + 2, 2).Value = order.Sender?.Username ?? "N/A";
                    ordersSheet.Cell(i + 2, 3).Value = order.Receiver?.Username ?? "N/A";
                    ordersSheet.Cell(i + 2, 4).Value = order.CargoName;
                    ordersSheet.Cell(i + 2, 5).Value = order.Quantity;
                    ordersSheet.Cell(i + 2, 6).Value = order.FromWarehouse?.WarehouseName ?? "N/A";
                    ordersSheet.Cell(i + 2, 7).Value = order.ToWarehouse?.WarehouseName ?? "N/A";
                    ordersSheet.Cell(i + 2, 8).Value = order.OrderDate;
                    ordersSheet.Cell(i + 2, 9).Value = order.Status;
                }

                // Populate Warehouses Sheet
                warehousesSheet.Cell(1, 1).Value = "WarehouseId";
                warehousesSheet.Cell(1, 2).Value = "WarehouseName";
                warehousesSheet.Cell(1, 3).Value = "Location";
                warehousesSheet.Cell(1, 4).Value = "Capacity";
                warehousesSheet.Cell(1, 5).Value = "MobileNum";

                for (int i = 0; i < warehouses.Count; i++)
                {
                    var warehouse = warehouses[i];
                    warehousesSheet.Cell(i + 2, 1).Value = warehouse.WarehouseId;
                    warehousesSheet.Cell(i + 2, 2).Value = warehouse.WarehouseName;
                    warehousesSheet.Cell(i + 2, 3).Value = warehouse.Location;
                    warehousesSheet.Cell(i + 2, 4).Value = warehouse.Capacity;
                    warehousesSheet.Cell(i + 2, 5).Value = warehouse.MobileNum;
                }
                cargosSheet.Cell(1, 1).Value = "CargoId";
                cargosSheet.Cell(1, 2).Value = "CargoName";
                cargosSheet.Cell(1, 3).Value = "Weight";
                cargosSheet.Cell(1, 4).Value = "Damaged";
                cargosSheet.Cell(1, 5).Value = "DateStored";
                cargosSheet.Cell(1, 6).Value = "WarehouseName";

                for (int i = 0; i < cargos.Count; i++)
                {
                    var cargo = cargos[i];
                    cargosSheet.Cell(i + 2, 1).Value = cargo.CargoId;
                    cargosSheet.Cell(i + 2, 2).Value = cargo.CargoName;
                    cargosSheet.Cell(i + 2, 3).Value = cargo.Weight;
                    cargosSheet.Cell(i + 2, 4).Value = cargo.Damaged;
                    cargosSheet.Cell(i + 2, 5).Value = cargo.DateStored;
                    cargosSheet.Cell(i + 2, 6).Value = cargo.Warehouse?.WarehouseName ?? "N/A";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
                }
            }
        }
    }

    public class OrdersByDate
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

}