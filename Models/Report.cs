using System;
using System.Collections.Generic;

namespace Cargo_FinalApplication.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public string ReportName { get; set; } = null!;

    public DateTime GeneratedDate { get; set; }

    public string ReportData { get; set; } = null!;
}
