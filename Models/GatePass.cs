using System;
using System.Collections.Generic;

namespace Cargo_FinalApplication.Models;

public partial class GatePass
{
    public int GatePassId { get; set; }

    public int? OrderId { get; set; }

    public DateTime DispatchDate { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Order? Order { get; set; }
}
