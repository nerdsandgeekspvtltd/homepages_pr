

namespace Sports.Blogs.Server.Models;

public partial class BlogPost
{
    public int Bpid { get; set; }

    public int? TenantId { get; set; }

    public string Title { get; set; } = null!;

    public string? ScopeType { get; set; }

    public string? Description { get; set; }

    public string? DetailItemId { get; set; }

    public string? Tags { get; set; }

    public DateTime? ValidTo { get; set; }

    public DateTime? ValidFrom { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Link { get; set; }

    public string? Division { get; set; }

    public string Discipline { get; set; } = null!;

    public DateTime? PublishedOn { get; set; }

    public string? HeaderImage { get; set; }

    public List<BlogAttribute> BlogAttributes { get; set; } = new List<BlogAttribute>();
    public virtual Tenant? Tenant { get; set; }

}
