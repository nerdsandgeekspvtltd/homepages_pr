using Darnton.Blazor.DeviceInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Sports.Events.WA.Data;
using Sports.Events.WA.Models;
using Sports.Events.WA.Services;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Kanban.Internal;

namespace Sports.Events.WA.Pages
{
    /// <summary>
    /// Component for the main index page displaying events.
    /// </summary>
    public partial class Index : ComponentBase
    {
        [Inject]
        IEventService eventService { get; set; } // Injecting the event service dependency

        [Inject]
        IJSRuntime JS { get; set; } // Injecting the JavaScript runtime dependency

        [Inject]
        IGeolocationService GeolocationService { get; set; } // Injecting the geolocation service dependency

        [Inject]
        IAzureMapJs azureMapJs { get; set; } // Injecting the Azure Map JavaScript service

        private Event eventList { get; set; } // Property to store the retrieved event data
        private string placeholderUrl = "https://images.unsplash.com/photo-1496128858413-b36217c2ce36?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3603&q=80"; // Default placeholder URL
        private EventFilters filters = new EventFilters(); // Property to store event filters
        private List<string> EventTags = new List<string>(); // List to store event tags
        private List<string> EventDivisions = new List<string>(); // List to store event divisions
        private List<string> EventDisciplines = new List<string>(); // List to store event disciplines
        private List<string> EventTypes = new List<string>(); // List to store event types
        public PaginationModel PaginationModel = new PaginationModel(); // Pagination model for event listing
        public string eventLocationType = ""; // Type of event location
        private List<Items> EventItems = new List<Items>(); // List of event items
        public string showMap { get; set; } // Flag to show the map
        protected GeolocationResult? CurrentPositionResult { get; set; } // Current geolocation result
        protected string CurrentLatitude => CurrentPositionResult?.Position?.Coords?.Latitude.ToString("F2"); // Current latitude
        protected string CurrentLongitude => CurrentPositionResult?.Position?.Coords?.Longitude.ToString("F2"); // Current longitude
        private object mapInstance { get; set; }


        /// <summary>
        /// Initializes the component.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            filters.TagIds = new List<string>();
            filters.DisciplinesIds = new List<string>();
            await LoadList(1);
            EventTags = eventList.Items.SelectMany(x => x.Tags).Distinct().ToList();
            EventDisciplines = eventList.Items.SelectMany(x => x.Disciplines).Distinct().ToList();
            EventDivisions = eventList.Items.SelectMany(x => x.Divisions).Distinct().ToList();
        }

        /// <summary>
        /// Handles actions after the component has rendered.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JS.InvokeVoidAsync("myBlazorInterop.setDotNetObj", DotNetObjectReference.Create(this));
                await azureMapJs.SetDotNetObj(DotNetObjectReference.Create(this));
                mapInstance = await azureMapJs.InitMap("map");
            }
        }

        /// <summary>
        /// Clears all map popups.
        /// </summary>
        public async Task ClearPopups()
        {
            await azureMapJs.ClearPopups();
        }

        /// <summary>
        /// Resets all filters to default values.
        /// </summary>
        public async void ResetForm()
        {
            filters.TagIds = new List<string>();
            filters.DivisionsIds = null;
            filters.DisciplinesIds = new List<string>();
            eventLocationType = null;
            await LoadList(0);
        }

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        public async void ViewAllEvents()
        {
            filters.PageSize = 0;
            await LoadList(0);
        }

        /// <summary>
        /// Handles the change event for division filters.
        /// </summary>
        private async void HandleDivsionFilterChange(ChangeEventArgs e)
        {
            filters.DivisionsIds = e.Value.ToString();
            await LoadList(1);
        }

        /// <summary>
        /// Adds events based on location.
        /// </summary>
        public async Task AddEventBasedOnLocation(double Longitude, double Latitude)
        {
            filters.EventIds.Clear();
            foreach (var item in eventList.Items)
            {
                var distance = Utlities.CalculateDistance(Latitude, Longitude, item.Location.Latitude, item.Location.Longitude);
                if (distance <= 300)
                {
                    await azureMapJs.SetPopUp($"<div style='padding:10px;color:white'><a href='Event/{item.Id}'> {item.Name.Substring(0, 20) + "..."}  </a></div>", item.Location.Longitude, item.Location.Latitude);
                    filters.EventIds.Add(item.Id);
                }
            }
            await LoadList(1);
            filters.EventIds.Clear();
        }

        /// <summary>
        /// Marks events near the current location on the map.
        /// </summary>
        public async Task MarkEventNearMe()
        {
            CurrentPositionResult = await GeolocationService.GetCurrentPosition();
            double lat = CurrentPositionResult.Position.Coords.Latitude;
            double lon = CurrentPositionResult.Position.Coords.Longitude;
            if (CurrentPositionResult.IsSuccess)
            {
                await azureMapJs.SetPopUp("<div style=\"padding:10px;color:white\">You're here</div>", lon, lat);
            }
            await AddEventBasedOnLocation(lat, lon);
        }

        /// <summary>
        /// Invoked when the map location changes.
        /// </summary>
        /// <param name="Longitude">The longitude of the new location.</param>
        /// <param name="Latitude">The latitude of the new location.</param>
        [JSInvokable]
        public async Task OnMapLocation(double Longitude, double Latitude)
        {
            // Check if eventLocationType is specified and is "OnMap"
            if (!string.IsNullOrEmpty(eventLocationType) && eventLocationType == "OnMap")
            {
                // Clear any existing popups on the map
                await azureMapJs.ClearPopups();

                // Add a marker to the map at the new location
                await azureMapJs.AddMarker(Longitude, Latitude);

                // Add events based on the new location
                await AddEventBasedOnLocation(Longitude, Latitude);
            }
        }


        /// <summary>
        /// Handles the event of changing the event location type.
        /// </summary>
        private async void HandleEventLocationTypeChange(ChangeEventArgs e)
        {
            eventLocationType = e.Value.ToString();
            if (eventLocationType == "NearByMe")
            {
                await ClearPopups();
                await MarkEventNearMe();
            }
            else if (eventLocationType == "OnMap")
            {
                await ClearPopups();
            }
        }

        /// <summary>
        /// Fetches events based on specified filters.
        /// </summary>
        public async Task FetchBlogs(EventFilters eventFilters)
        {
            eventList = await eventService.GetAllEvents(eventFilters);
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Loads events based on specified page.
        /// </summary>
        private async Task LoadList(int Page)
        {
            filters.Page = Page;
            await FetchBlogs(filters);
            PaginationModel.TotalItems = eventList.Maximumcount;
        }

        /// <summary>
        /// Fetches events on change.
        /// </summary>
        public async void FetchEventsOnChange(ClosedEventArgs args)
        {
            await LoadList(1);
        }

        /// <summary>
        /// Searches events by keyword.
        /// </summary>
        public async void SearchBlogByKeyword(ChangeEventArgs e)
        {
            filters.Keyword = e.Value.ToString() == "" ? null : e.Value.ToString();
            await LoadList(1);
        }

        /// <summary>
        /// Handles the event of clearing the handler.
        /// </summary>
        private async void Clearedhandler(MouseEventArgs args)
        {
            await LoadList(1);
        }
    }
}
