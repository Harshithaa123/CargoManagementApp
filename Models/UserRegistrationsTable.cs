using System;
using System.Collections.Generic;

namespace Cargo_FinalApplication.Models;

public partial class UserRegistrationsTable
{
    public int RegistrationId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }
}
