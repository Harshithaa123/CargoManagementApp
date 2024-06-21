using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cargo_FinalApplication.Models;

public partial class WarehouseInventory
{
    public int WarehouseId { get; set; }

    public int CargoId { get; set; }

    public int Quantity { get; set; }
    [JsonIgnore]
    public virtual Cargo Cargo { get; set; } = null!;
    [JsonIgnore]
    public virtual Warehouse Warehouse { get; set; } = null!;
}
