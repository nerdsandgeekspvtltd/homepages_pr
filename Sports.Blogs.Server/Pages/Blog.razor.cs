using Sports.Blogs.Server.Services.BlogPage;
using Sports.Blogs.Server.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.RichTextEditor;


namespace Sports.Blogs.Server.Pages
{
    public partial class Blog
    {
        // Parameter to receive the link of the blog page
        [Parameter]
        public string BpId { get; set; }

        // Injected dependency for accessing blog page data
        [Inject]
        IBlogPageService BlogPageService { get; set; }

        // Injected NavigationManager for navigation
        [Inject]
        private NavigationManager Navigation { get; set; }

        // Injected JavaScript runtime dependency for interacting with JavaScript
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        //// Property to store the retrieved list of blog pages
        //private BlogsList blogsList { get; set; }

        //// Property to store the specific blog item
        //private Item BlogItem { get; set; }

        // Property to store blog post
        private BlogPost blogPost { get; set; }

        /// <summary>
        /// Method called when the component is initialized asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Call the base implementation of OnInitializedAsync
            await base.OnInitializedAsync();

            // Check if the BpId parameter is provided
            if (!string.IsNullOrEmpty(BpId))
            {
                // Retrieve blog post
                blogPost = BlogPageService.GetBlogPageById(Convert.ToInt32(BpId));
            }

            // Check if the BpId parameter is provided or if the blog Post is found (indicating no matching blog found)
            if (string.IsNullOrEmpty(BpId) || blogPost.Bpid == 0)
            {
                // Redirect to the default BlogsHome page if no link is provided
                Navigation.NavigateTo("/blogs");
            }
        }

        /// <summary>
        /// Method called after the component has rendered.
        /// </summary>
        /// <param name="firstRender">A boolean indicating whether this is the first render of the component.</param>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Check if this is the first render of the component
            if (firstRender)
            {
                // Invoke a JavaScript function to check for broken images, passing the placeholder URL
                await JSRuntime.InvokeVoidAsync("imageChecker.checkBrokenImages", Utilities.placeholderUrl);
            }
        }


    }

}
