using System;
using System.Collections.Generic;

namespace Cargo_FinalApplication.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string WarehouseName { get; set; } = null!;

    public string Location { get; set; } = null!;

    public int Capacity { get; set; }

    public string MobileNum { get; set; } = null!;

    public virtual ICollection<Cargo> Cargos { get; set; } = new List<Cargo>();

    public virtual ICollection<Order> OrderFromWarehouses { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderToWarehouses { get; set; } = new List<Order>();

    public virtual ICollection<WarehouseInventory> WarehouseInventories { get; set; } = new List<WarehouseInventory>();
}
