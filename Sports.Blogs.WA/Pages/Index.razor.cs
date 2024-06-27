using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Sports.Blogs.WA.Models;
using Sports.Blogs.WA.Services;
using Sports.Blogs.WA.Services.BlogPage;
using Syncfusion.Blazor.DropDowns;
using Microsoft.AspNetCore.Components.Web;

namespace Sports.Blogs.WA.Pages
{
    public partial class Index : ComponentBase
    {
        // Injecting the BlogPageService dependency
        [Inject]
        IBlogPageService BlogPageService { get; set; }

        // Injecting the JavaScript runtime dependency
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        // Property to store the retrieved blog data
        private List<BlogPost> blogsPosts { get; set; }

        // Property to store blog response 
        private BlogPostResponse blogPostResponse { get; set; }

        // Property to store blog filters
        private BlogFilters filters = new BlogFilters();

        // List to store blog tags
        private List<string> BlogTags = new List<string>();

        // List to store blog attributes
        private List<BlogFilterSuggestions> blogAttributes = new List<BlogFilterSuggestions>();

        // List to store blog divisions
        private List<string> BlogDivisions = new List<string>();

        // List to store blog disciplines
        private List<string> BlogDisciplines = new List<string>();

        // Model to handle pagination
        public PaginationModel PaginationModel = new PaginationModel();

        public bool DisablePagination = false;

        // Property to show/hide spinner 
        public bool IsSpinnerVisible { get; set; } = true;

        /// <summary>
        /// Method invoked when the component is initialized asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Call the base implementation of OnInitializedAsync
            await base.OnInitializedAsync();

            // Initialize the TagIds filter with an empty list
            filters.TagIds = new List<string>();

            // Initializing the search fields 
            filters.SearchFields = "Title";

            // Initializing the blogs list page size 
            filters.PageSize = 20;

            // Load the list of blog entries with pagination starting from page 1
            await LoadList(1);

            // Load the blog attributes
            await FetchBlogAttributes();

            //  Checking if blog attributes exists
            if (blogAttributes.Any())
            {
                // Add distinct tags, disciplines, and divisions from the loaded blog entries to their respective lists
                BlogTags.AddRange(Utilities.GetBlogAttributes(blogAttributes, "tag"));
                BlogDisciplines.AddRange(Utilities.GetBlogAttributes(blogAttributes, "discipline"));
                BlogDivisions.AddRange(Utilities.GetBlogAttributes(blogAttributes, "division"));
            }

            await InvokeAsync(StateHasChanged);
        }


        public async void ViewAllBlogs()
        {
            filters.PageSize = 0;
            DisablePagination = false;

            await LoadList(0);
        }

        public async void ResetForm()
        {
            filters.TagIds = new List<string>();
            filters.DivisionIds = null;
            filters.DisciplineIds = new List<string>();
            filters.Keyword = null;
            await LoadList(0);
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        // Invoking JavaScript function and passing the placeholder URL
        //        await JSRuntime.InvokeVoidAsync("imageChecker.checkBrokenImages", Utilities.placeholderUrl);
        //    }
        //}

        /// <summary>
        /// Method to fetch blogs when filters (tags, division, and disciplines) change.
        /// </summary>
        /// <param name="blogFilters">The filters to apply for fetching blogs.</param>
        public async Task FetchBlogs(BlogFilters blogFilters)
        {
            // Fetch blogs list based on the provided filters
            blogPostResponse = await BlogPageService.GetAllBlogPages(blogFilters);

            // check if blog post is null & if any blog posts exists
            if (blogPostResponse.blogPosts != null && blogPostResponse.blogPosts.Any())
            {
                blogsPosts = blogPostResponse.blogPosts;
                IsSpinnerVisible = false;
            }

            // Invoke the component's state change to update UI
            await InvokeAsync(StateHasChanged);
        }

        private async void HandleDivsionFilterChange(Microsoft.AspNetCore.Components.ChangeEventArgs e)
        {
            filters.DivisionIds = e.Value.ToString();
            await LoadList(1);
        }


        /// <summary>
        /// Method to blog attributes.
        /// </summary>
        public async Task FetchBlogAttributes()
        {

            // Fetch blogs attributes based on the tenant id
            blogAttributes = await BlogPageService.GetBlogFilterSuggestions(0);
        }

        /// <summary>
        /// Method to load the list of blogs for a specified page.
        /// </summary>
        /// <param name="Page">The page number for which the blog list should be loaded.</param>
        private async Task LoadList(int Page)
        {
            // Set the page number in the filters
            filters.Page = Page;

            // Fetch blogs list for the specified page
            await FetchBlogs(filters);

            // Update total items count in pagination model
            PaginationModel.TotalItems = blogPostResponse.MaximumCount;

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Method to fetch blogs when the selected filters are changed.
        /// </summary>
        /// <param name="args">Arguments containing information about the changed filters.</param>
        public async void FetchBlogsOnChange(ClosedEventArgs args)
        {
            // Load the list of blogs for the first page
            await LoadList(1);
        }

        /// <summary>
        /// Method to search blogs by keyword.
        /// </summary>
        /// <param name="e">Event arguments containing the keyword for search.</param>
        public async void SearchBlogByKeyword(ChangeEventArgs e)
        {
            // Update the keyword filter based on the search input
            filters.Keyword = e.Value.ToString() == "" ? null : e.Value.ToString();

            // Load the list of blogs for the first page
            await LoadList(1);
        }

        private async void OnInputClear(MouseEventArgs args)
        {
            // Load the list of blogs for the first page
            await LoadList(1);
        }



    }
}
