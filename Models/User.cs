using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cargo_FinalApplication.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<GatePass> GatePasses { get; set; } = new List<GatePass>();
    [JsonIgnore]
    public virtual ICollection<Order> OrderReceivers { get; set; } = new List<Order>();
    [JsonIgnore]
    public virtual ICollection<Order> OrderSenders { get; set; } = new List<Order>();
    [JsonIgnore]
    public virtual ICollection<UserDetail> UserDetails { get; set; } = new List<UserDetail>();
}
