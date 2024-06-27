using System;
using System.Collections.Generic;

namespace Sports.Blogs.WA.Models;

public partial class Tenant
{
    public int TenantId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }
}
