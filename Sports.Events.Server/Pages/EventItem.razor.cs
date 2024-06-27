using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Sports.Events.Server.Data;
using Sports.Events.Server.Services;
using Syncfusion.Blazor.RichTextEditor;
using Syncfusion.Blazor.RichTextEditor.Internal;
using Sports.Events.Server.Models;
using Darnton.Blazor.DeviceInterop.Geolocation;
using AzureMapsControl.Components.Map;
using Microsoft.IdentityModel.Abstractions;
using Syncfusion.Blazor.Gantt.Internal;

namespace Sports.Events.Server.Pages
{
    public partial class EventItem
    {
        // Parameter to receive the link of the blog page
        [Parameter]
        public string Id { get; set; }


        // Injected dependency for accessing blog page data
        [Inject]
        IEventService eventService { get; set; }

        // Injected NavigationManager for navigation
        [Inject]
        private NavigationManager Navigation { get; set; }

        // Injected JavaScript runtime dependency for interacting with JavaScript
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        // Property to store the retrieved list of Event pages
        private Event events { get; set; }

        // Property to store the specific blog item
        private Items eventDetail { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; }

        [Inject]
        private IConfiguration _configuration { get; set; }

        [Inject]
        private IGeolocationService GeolocationService { get; set; }

        private EventLocation eventLocation { get; set; }
        // object map { get; set; }


        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        //await JSRuntime.InvokeVoidAsync("imageChecker.checkBrokenImages", Utilities.placeholderUrl);

        //        // mapModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./leafletInterop.js");
        //        map = await JSRuntime.InvokeAsync<IJSObjectReference>("window.mapInterop.initMap", "map", "13.6288", "79.4192"); // Pass the ID of the map element

        //        // // Example: Add marker
        //        // await mapModule.InvokeVoidAsync("addMarker", map, 51.5, -0.09);

        //        // // Example: Get markers
        //        // var markers = await mapModule.InvokeAsync<IEnumerable<location>>("getMarkers", map);
        //        // foreach (var marker in markers)
        //        // {
        //        //     Console.WriteLine($"Latitude: {marker.latitude}, Longitude: {marker.longitude}");
        //        // }
        //    }
        //}

        /// <summary>
        /// Method called when the component is initialized asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Call the base implementation of OnInitializedAsync
            await base.OnInitializedAsync();

            // Check if the link parameter is provided
            if (Id != null)
            {
                // Retrieve all blog pages
                events = await eventService.GetAllEvents(new EventFilters());

                // Find the specific blog item with the provided link
                if (events != null)
                {

                    eventDetail = events.Items.FirstOrDefault(x => x.Id == Id) ?? new();
                    eventLocation = eventDetail.Location;
                    eventLocation.Name = eventDetail.Name;
                    // await JSRuntime.InvokeVoidAsync("mapInterop.addMarker", map, eventDetail.Location.Latitude, eventDetail.Location.Longitude);
                }
            }

            // Check if the link parameter is provided or if the BlogItem is null (indicating no matching blog found)
            if (Id == null || eventDetail.Id == null)
            {
                // Redirect to the default BlogsHome page if no link is provided
                Navigation.NavigateTo("/EventList");
            }
        }

        public async Task OnMapReadyAsync(MapEventArgs eventArgs)
        {

            if (eventLocation != null)
            {
                await eventArgs.Map.SetCameraOptionsAsync(
                    options => options.Center =
                    new AzureMapsControl.Components.Atlas.Position
                    (eventLocation.Longitude, eventLocation.Latitude));
                await eventArgs.Map.SetTrafficOptionsAsync(options => options.Incidents = true);

                var marker = new AzureMapsControl.Components.Markers.HtmlMarker(
            new AzureMapsControl.Components.Markers.HtmlMarkerOptions
            {
                Position = new AzureMapsControl.Components.Atlas.Position(eventLocation.Longitude, eventLocation.Latitude),
                Draggable = true
            },
            AzureMapsControl.Components.Markers.HtmlMarkerEventActivationFlags.None().Enable(AzureMapsControl.Components.Markers.HtmlMarkerEventType.Click));

                await eventArgs.Map.AddHtmlMarkersAsync(marker);
            }
        }


        private async Task JoinEvent()
        {
            // For testing purpose, navigate to a simple join page
            Navigation.NavigateTo("/join");
        }
    }
}
