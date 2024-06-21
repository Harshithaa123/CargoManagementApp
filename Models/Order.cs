using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cargo_FinalApplication.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? SenderId { get; set; }

    public int? ReceiverId { get; set; }

    public string CargoName { get; set; } = null!;

    public int Quantity { get; set; }

    public int? FromWarehouseId { get; set; }

    public int? ToWarehouseId { get; set; }

    public DateTime OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public bool IsOutgoing { get; set; }
    [JsonIgnore]
    public virtual Warehouse? FromWarehouse { get; set; }
    [JsonIgnore]
    public virtual ICollection<GatePass> GatePasses { get; set; } = new List<GatePass>();
    [JsonIgnore]
    public virtual User? Receiver { get; set; }
    [JsonIgnore]
    public virtual User? Sender { get; set; }
    [JsonIgnore]
    public virtual Warehouse? ToWarehouse { get; set; }
}
