using Sports.Blogs.WA.Models;

namespace Sports.Blogs.WA
{
    public class Utilities
    {

        // Default placeholder URL for blog images
        public static string placeholderUrl = "https://images.unsplash.com/photo-1496128858413-b36217c2ce36?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3603&q=80";

        /// <summary>
        /// Gets blog attributes based on their type
        /// </summary>
        /// <param name="blogAttributes">The list of blog attributes.</param>
        /// <param name="attributeType">The type of blog attribute.</param>
        public static List<string> GetBlogAttributes(List<BlogFilterSuggestions> blogAttributes, string attributeType)
        {
            return blogAttributes.Where(x => x.FilterType == attributeType).Select(x => x.FilterValue).Distinct().ToList();
        }

        public static string CheckIfImageisEmpty(string ImageURL)
        {
            return (string.IsNullOrEmpty(ImageURL)) ? placeholderUrl : ImageURL;
        }

        /// <summary>
        /// Gets the URL of the blog image from the list of components.
        /// </summary>
        /// <param name="components">The list of components containing the blog content.</param>
        /// <returns>The URL of the blog image, or a placeholder URL if not found.</returns>
        //public static string GetBlogImage(List<Component> components)
        //{
        //    // Get the URL of the image component or use the placeholder URL if not found
        //    return components.Where(x => x.ComponentType == "image").Select(x => x.ImageUrl).FirstOrDefault() ?? placeholderUrl;
        //}

        /// <summary>
        /// Gets the content of the blog from the list of components.
        /// </summary>
        /// <param name="components">The list of components containing the blog content.</param>
        /// <returns>The content of the blog, or an empty string if not found.</returns>
        //public static string GetBlogContent(List<Component> components)
        //{
        //    // Get the content of the text component or return an empty string if not found
        //    return components.Where(x => x.ComponentType == "text").Select(x => x.Content).FirstOrDefault() ?? "";
        //}
    }
}
