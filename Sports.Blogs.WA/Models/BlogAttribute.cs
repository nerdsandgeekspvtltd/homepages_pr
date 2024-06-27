﻿using System;
using System.Collections.Generic;

namespace Sports.Blogs.WA.Models;

public partial class BlogAttribute
{
    public int Baid { get; set; }

    public int? BpId { get; set; }

    public string? AttributeTitle { get; set; }

    public string? AttributeType { get; set; }

    public virtual BlogPost? Bp { get; set; }
}
