using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cargo_FinalApplication.Models;

public partial class Cargo
{
    public int CargoId { get; set; }

    public string CargoName { get; set; } = null!;

    public decimal Weight { get; set; }

    public bool Damaged { get; set; }

    public DateTime DateStored { get; set; }

    public int? WarehouseId { get; set; }
    [JsonIgnore]
    public virtual Warehouse? Warehouse { get; set; }
    [JsonIgnore]
    public virtual ICollection<WarehouseInventory> WarehouseInventories { get; set; } = new List<WarehouseInventory>();
}
