using Microsoft.AspNetCore.Components;
using Sports.Events.WA.Models;

namespace Sports.Events.WA.Pages
{
    /// <summary>
    /// Component responsible for handling pagination functionality.
    /// </summary>
    public partial class Pagination : ComponentBase
    {
        /// <summary>
        /// Gets or sets the pagination model containing pagination data.
        /// </summary>
        [Parameter]
        public PaginationModel paginationModel { get; set; }

        /// <summary>
        /// Event callback to be invoked when the page changes.
        /// </summary>
        [Parameter]
        public EventCallback<int> PageChanged { get; set; }

        /// <summary>
        /// Changes the current page to the specified page number.
        /// </summary>
        /// <param name="page">The page number to navigate to.</param>
        private async Task ChangePage(int page)
        {
            if (paginationModel.CurrentPage != page)
            {
                paginationModel.CurrentPage = page;
                await PageChanged.InvokeAsync(page);
            }
        }

        /// <summary>
        /// Navigates to the previous page if available.
        /// </summary>
        private async Task PreviousPage()
        {
            if (paginationModel.HasPreviousPage)
            {
                paginationModel.CurrentPage--;
                await PageChanged.InvokeAsync(paginationModel.CurrentPage);
            }
        }

        /// <summary>
        /// Navigates to the next page if available.
        /// </summary>
        private async Task NextPage()
        {
            if (paginationModel.HasNextPage)
            {
                paginationModel.CurrentPage++;
                await PageChanged.InvokeAsync(paginationModel.CurrentPage);
            }
        }
    }
}
