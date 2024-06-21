using System;
using System.Collections.Generic;

namespace Cargo_FinalApplication.Models;

public partial class UserDetail
{
    public int UserDetailId { get; set; }

    public int? UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual User? User { get; set; }
}
