using Sports.Blogs.Server.Models;

namespace Sports.Blogs.Server.Services.BlogPage
{
    // Interface for a service that provides operations related to blog pages.
    public interface IBlogPageService
    {
        // Asynchronously retrieves a list of all blog pages.
        Task<BlogPostResponse> GetAllBlogPages(BlogFilters blogFilters);

        // Retrieves a list of all blog suggestions.
        List<BlogFilterSuggestions> GetBlogFilterSuggestions(int TenantId);

        // Retrives a blog based on it's Id 
        BlogPost GetBlogPageById(int Id);
    }
}
