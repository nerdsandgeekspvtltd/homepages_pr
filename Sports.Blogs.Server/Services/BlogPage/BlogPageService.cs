using Sports.Blogs.Server.Services.BlogPage;
using Sports.Blogs.Server.Models;
using Sports.Blogs.Server.Data;
using Syncfusion.Windows.Forms;
using Syncfusion.Windows.Forms.Grid;

namespace Sports.Blogs.Server.Services.BlogPage
{
    public class BlogPageService : IBlogPageService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPageService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used for making requests to the API.</param>
        public BlogPageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves a list of blog entries based on provided filters from an API endpoint.
        /// </summary>
        /// <param name="blogFilters">The filters to be applied to the blog entries.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved or empty BlogsList.</returns>
        public async Task<BlogPostResponse> GetAllBlogPages(BlogFilters blogFilters)
        {

            // Declare an instance of BlogsList
            BlogPostResponse blogPostResponse;

            // Initialize query string
            var query = "?";

            // Check if FromDate filter is provided
            if (blogFilters.FromDate != null)
                query += $"from={Uri.EscapeDataString(blogFilters.FromDate.ToString())}&";

            // Check if Page filter is provided
            if (blogFilters.Page > 0)
                query += $"page={blogFilters.Page}&";

            // Check if PageSize filter is provided
            if (blogFilters.PageSize > 0)
                query += $"PageSize={blogFilters.PageSize}&";

            // Check if OrderBy filter is provided
            if (!string.IsNullOrEmpty(blogFilters.OrderBy))
                query += $"orderby={Uri.EscapeDataString(blogFilters.OrderBy)}&";

            // Check if Keyword filter is provided
            if (!string.IsNullOrEmpty(blogFilters.Keyword))
                query += $"keyword={Uri.EscapeDataString(blogFilters.Keyword)}&";

            // Check if SearchFields filter is provided
            if (blogFilters.SearchFields != null && blogFilters.SearchFields.Any())
                query += $"searchfields={Uri.EscapeDataString(string.Join(",", blogFilters.SearchFields))}&";

            //// Check if DivisionIds filter is provided
            //if (blogFilters.DivisionIds != null && blogFilters.DivisionIds.Any())
            //    query += $"DivisionsIds={Uri.EscapeDataString(string.Join(",", blogFilters.DivisionIds))}&";

            // Check if DivisionIds filter is provided
            if (!string.IsNullOrEmpty(blogFilters.DivisionIds))
                query += $"DivisionsIds={Uri.EscapeDataString(blogFilters.DivisionIds)}&";

            // Check if DisciplineIds filter is provided
            if (blogFilters.DisciplineIds != null && blogFilters.DisciplineIds.Any())
                query += $"DisciplinesIds={Uri.EscapeDataString(string.Join(",", blogFilters.DisciplineIds))}&";

            // Check if TagIds filter is provided
            if (blogFilters.TagIds != null && blogFilters.TagIds.Any())
                query += $"TagIds={Uri.EscapeDataString(string.Join(",", blogFilters.TagIds))}&";

            // Check if CreatorIds filter is provided
            if (blogFilters.CreatorIds != null && blogFilters.CreatorIds.Any())
                query += $"CreatorIds={Uri.EscapeDataString(string.Join(",", blogFilters.CreatorIds).TrimEnd(','))}";

            // Remove the trailing '&' if any
            query = query.TrimEnd('&');

            try
            {
                // Send HTTP GET request with the constructed query string to the specified API endpoint
                blogPostResponse = await _httpClient.GetFromJsonAsync<BlogPostResponse>("BlogPage/GetAllBlogPages" + query) ?? new();
                //var response = await _httpClient.GetAsync("BlogPage/GetAllBlogPages" + query) ?? new();
                //var resp = response.Content.ReadAsStringAsync();
                //var result = await response.Content.ReadFromJsonAsync<BlogPostResponse>();
                //blogPostResponse = result;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the HTTP request
                Console.WriteLine(ex.Message);
                // If an exception occurs, initialize an empty BlogsList
                blogPostResponse = new();
            }

            // Return the retrieved or empty BlogsList
            return blogPostResponse;


            /*
            // Declare an instance of BlogsList
            BlogsList blogList;

            try
            {
                // Send HTTP GET request with the constructed query string to the specified API endpoint
                blogList = await _httpClient.GetFromJsonAsync<BlogsList>("https://api.search.sportcloud.de/api/blogentry?page=1&pagesize=25&orderbytype=descending&orderby=CreatedAt") ?? new BlogsList();

                foreach (var item in blogList.Items)
                {
                    // Initialize min and max for random selection
                    var min = 0;
                    var max = BlogTags.Tags.Count;
                    Random rand = new Random();

                    //initialing an empty division list
                    item.Divisions = new List<string>();

                    //initialing an empty discipline list
                    item.Disciplines = new List<string>();

                    // Add random tags, divisions, and disciplines from BlogTags to each blog item
                    item.Tags.AddRange(BlogTags.Tags.ElementAt(rand.Next(min, max)));
                    item.Divisions.AddRange(BlogTags.Tags.ElementAt(rand.Next(min, max)));
                    item.Disciplines.AddRange(BlogTags.Tags.ElementAt(rand.Next(min, max)));
                }

                // Filter blogList.Items based on TagIds filter
                if (blogFilters.TagIds != null && blogFilters.TagIds.Any())
                {
                    blogList.Items = blogList.Items.Where(x => blogFilters.TagIds.Intersect(x.Tags).Any()).ToList();
                }

                // Filter blogList.Items based on DivisionIds filter
                if (blogFilters.DivisionIds != null && blogFilters.DivisionIds.Any())
                {
                    blogList.Items = blogList.Items.Where(x => blogFilters.DivisionIds.Intersect(x.Divisions).Any()).ToList();
                }

                // Filter blogList.Items based on DisciplineIds filter
                if (blogFilters.DisciplineIds != null && blogFilters.DisciplineIds.Any())
                {
                    blogList.Items = blogList.Items.Where(x => blogFilters.DisciplineIds.Intersect(x.Disciplines).Any()).ToList();
                }

                // Filter blogList.Items based on Keyword filter
                if (!string.IsNullOrEmpty(blogFilters.Keyword))
                {
                    blogList.Items = blogList.Items.Where(x => x.Title.Contains(blogFilters.Keyword.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Set Maximumcount property of blogList to the count of filtered items
                blogList.Maximumcount = blogList.Items.Count;

                // Paginate the filtered items based on page number and take 18 items per page
                blogList.Items = blogList.Items.Skip((blogFilters.Page - 1) * 18).Take(18).ToList();

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the HTTP request
                Console.WriteLine(ex.Message);
                // If an exception occurs, initialize an empty BlogsList
                blogList = new BlogsList();
            }

            // Return the retrieved or empty BlogsList
            return blogList;
            */
        }

        /// <summary>
        /// Retrieves suggestions for blog filter options based on the provided TenantId .
        /// </summary>
        /// <param name="TenantId">The identifier of the tenant.</param>
        public List<BlogFilterSuggestions> GetBlogFilterSuggestions(int TenantId)
        {
            // Send HTTP GET request with the constructed query string to the specified API endpoint
            return _httpClient.GetFromJsonAsync<List<BlogFilterSuggestions>>("BlogPage/GetBlogFilterSuggestions/" + TenantId).Result ?? new();
        }

        /// <summary>
        /// Retrieves blog post based on blog Id.
        /// </summary>
        /// <param name="Id">The identifier of the blog.</param>
        public BlogPost GetBlogPageById(int Id)
        {
            // Declare an instance of Blog Post
            BlogPost blogPost;
            try
            {
                // Send HTTP GET request with the constructed query string to the specified API endpoint
                blogPost = _httpClient.GetFromJsonAsync<BlogPost>("BlogPage/GetBlogPageById/" + Id).Result ?? new();
            }
            catch (Exception ex)
            {
                blogPost = new();
            }

            return blogPost;

        }
    }
}
