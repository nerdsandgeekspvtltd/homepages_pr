using AzureMapsControl.Components.Map;
using Darnton.Blazor.DeviceInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Identity.Client;
using Microsoft.JSInterop;
using Sports.Events.Server.Data;
using Sports.Events.Server.Models;
using Sports.Events.Server.Services;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.PivotView;

namespace Sports.Events.Server.Pages
{
    public partial class EventList
    {
        [Inject]
        IEventService eventService { get; set; } // Injecting the BlogPageService dependency

        [Inject]
        IJSRuntime JSRuntime { get; set; } // Injecting the JavaScript runtime dependency

        [Inject]
        IConfiguration _configuration { get; set; }
        [Inject]
        IGeolocationService GeolocationService { get; set; }

        private Event eventList { get; set; } // Property to store the retrieved blog data
        private string placeholderUrl = "https://images.unsplash.com/photo-1496128858413-b36217c2ce36?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3603&q=80"; // Default placeholder URL
        private EventFilters filters = new EventFilters(); // Property to store blogfilters
        private List<string> EventTags = new List<string>();
        private List<string> EventDivisions = new List<string>();
        private List<string> EventDisciplines = new List<string>();
        private List<string> EventTypes = new List<string>();
        public PaginationModel PaginationModel = new PaginationModel();
        public string eventLocationType = "";
        private AzureMapsControl.Components.Markers.HtmlMarker _marker;
        public string showMap { get; set; } // Flag to show the map

        protected GeolocationResult? CurrentPositionResult { get; set; }
        protected string CurrentLatitude =>
        CurrentPositionResult?.Position?.Coords?.Latitude.ToString("F2");
        protected string CurrentLongitude =>
        CurrentPositionResult?.Position?.Coords?.Longitude.ToString("F2");
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                CurrentPositionResult = await GeolocationService.GetCurrentPosition();
                StateHasChanged();
            }
        }

        public async void ResetForm()
        {
            filters.TagIds = new List<string>();
            filters.DivisionsIds = null;
            filters.DisciplinesIds = new List<string>();
            eventLocationType = null;
            filters.Keyword = null;

            await LoadList(0);
        }
        public async void ViewAllEvents()
        {
            filters.PageSize = 0;
            //DisablePagination = false;
            await LoadList(0);
        }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();



            filters.TagIds = new List<string>();
            //filters.DivisionsIds = new List<string>();
            filters.DisciplinesIds = new List<string>();


            // Initializing the blogs list page size 
            filters.PageSize = 20;


            await LoadList(1);
            eventList.Items.ForEach(x => EventTags.AddRange(x.Tags));
            eventList.Items.ForEach(x => EventDisciplines.AddRange(x.Disciplines));
            eventList.Items.ForEach(x => EventDivisions.AddRange(x.Divisions));

            EventTags = EventTags.Distinct().ToList();
            EventDisciplines = EventDisciplines.Distinct().ToList();
            EventDivisions = EventDivisions.Distinct().ToList();

        }

        private async void HandleDivsionFilterChange(Microsoft.AspNetCore.Components.ChangeEventArgs e)
        {
            filters.DivisionsIds = e.Value.ToString();
            await LoadList(1);
        }

        public async Task NearMeOnMapReadyAsync(MapEventArgs eventArgs)
        {
            CurrentPositionResult = await GeolocationService.GetCurrentPosition();
            if (CurrentPositionResult.IsSuccess)
            {
                await eventArgs.Map.SetCameraOptionsAsync(
                    options => options.Center =
                    new AzureMapsControl.Components.Atlas.Position
                    (Convert.ToDouble(CurrentLongitude), Convert.ToDouble(CurrentLatitude)));
                await eventArgs.Map.SetTrafficOptionsAsync(options => options.Incidents = true);

                await eventArgs.Map.AddHtmlMarkersAsync
           (
               new AzureMapsControl.Components.Markers.HtmlMarker(
               new AzureMapsControl.Components.Markers.HtmlMarkerOptions
               {

                   Position = new AzureMapsControl.Components.Atlas.Position(Convert.ToDouble(CurrentLongitude), Convert.ToDouble(CurrentLatitude)),
                   Draggable = false
               })
           );
            }
        }

        public async Task OnMapClick(MapMouseEventArgs args)
        {

            if (eventLocationType == "OnMap")
            {
                if (_marker != null)
                {
                    //await args.Map.RemoveHtmlMarkersAsync(_marker);
                    await args.Map.ClearHtmlMarkersAsync();
                }

                _marker = new AzureMapsControl.Components.Markers.HtmlMarker(
                    new AzureMapsControl.Components.Markers.HtmlMarkerOptions
                    {
                        Position = args.Position
                    }, AzureMapsControl.Components.Markers.HtmlMarkerEventActivationFlags.None());
                await args.Map.AddHtmlMarkersAsync(_marker);

                filters = new EventFilters();

                foreach (var item in eventList.Items)
                {
                    var distance = Utlities.CalculateDistance(args.Position.Latitude, args.Position.Longitude, item.Location.Latitude, item.Location.Longitude);

                    if (distance <= 300)
                    {
                        var marker = new AzureMapsControl.Components.Markers.HtmlMarker(
                    new AzureMapsControl.Components.Markers.HtmlMarkerOptions
                    {
                        //Text = item.Name,
                        Color = "red",
                        Position = new AzureMapsControl.Components.Atlas.Position(item.Location.Longitude, item.Location.Latitude),
                        Draggable = false
                    },
                    AzureMapsControl.Components.Markers.HtmlMarkerEventActivationFlags.None().Enable(AzureMapsControl.Components.Markers.HtmlMarkerEventType.Click));

                        await args.Map.AddHtmlMarkersAsync(marker);

                        filters.TagIds.AddRange(item.Tags);
                    }

                }

                await LoadList(1);

            }

        }

        public async Task OnMapReadyAsync(MapEventArgs eventArgs)
        {


            await eventArgs.Map.SetCameraOptionsAsync(
                options => options.Center =
                new AzureMapsControl.Components.Atlas.Position
                (79.4192, 13.6288));
            await eventArgs.Map.SetTrafficOptionsAsync(options => options.Incidents = true);

            //    var marker = new AzureMapsControl.Components.Markers.HtmlMarker(
            //new AzureMapsControl.Components.Markers.HtmlMarkerOptions
            //{
            //    Position = new AzureMapsControl.Components.Atlas.Position(eventLocation.Longitude, eventLocation.Latitude),
            //    Draggable = true
            //},
            //AzureMapsControl.Components.Markers.HtmlMarkerEventActivationFlags.None().Enable(AzureMapsControl.Components.Markers.HtmlMarkerEventType.Click));

            //    await eventArgs.Map.AddHtmlMarkersAsync(marker);

        }

        private void HandleEventLocationTypeChange(Microsoft.AspNetCore.Components.ChangeEventArgs e)
        {
            // Update the eventLocationType property with the selected value
            eventLocationType = e.Value.ToString();
        }

        // Method to fetch blogs on filters (tags, division & disciplines) change
        public async Task FetchBlogs(EventFilters eventFilters)
        {
            eventList = await eventService.GetAllEvents(eventFilters);
            await InvokeAsync(StateHasChanged);
        }

        private async Task LoadList(int Page)
        {
            filters.Page = Page;
            await FetchBlogs(filters);
            PaginationModel.TotalItems = eventList.Maximumcount;
        }


        public async void FetchEventsOnChange(ClosedEventArgs args)
        {
            await LoadList(1);
        }

        public async void SearchBlogByKeyword(ChangeEventArgs e)
        {
            filters.Keyword = e.Value.ToString() == "" ? null : e.Value.ToString();
            await LoadList(1);
        }


        private async void Clearedhandler(MouseEventArgs args)
        {
            await LoadList(1);
        }
    }
}
