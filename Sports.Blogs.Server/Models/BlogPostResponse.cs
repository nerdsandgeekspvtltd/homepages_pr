


namespace Sports.Blogs.Server.Models;
    public class BlogPostResponse
    {
        public List<BlogPost> blogPosts { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int MaximumCount { get; set; }
    }
