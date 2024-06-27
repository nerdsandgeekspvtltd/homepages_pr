using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Sports.Events.Server.Data;
using Sports.Events.Server.Models;
using Sports.Events.Server.Services;
using Syncfusion.Blazor.Maps;

namespace Sports.Events.Server.Pages
{
    public partial class EventMaps
    {
        [Inject]
        IEventService eventService { get; set; } // Injecting the BlogPageService dependency

        [Inject]
        IJSRuntime JSRuntime { get; set; } // Injecting the JavaScript runtime dependency
        private Event eventList { get; set; } // Property to store the retrieved blog data
        private List<EventLocation> eventLocations { get; set; } = new List<EventLocation>();
        private List<Items> filteredItems = new List<Items>();
        object map { get; set; }

        private EventLocation clientLocation { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await JSRuntime.InvokeVoidAsync("imageChecker.checkBrokenImages", Utilities.placeholderUrl);

                // mapModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./leafletInterop.js");
                map = await JSRuntime.InvokeAsync<IJSObjectReference>("window.mapInterop.initMap", "map", "13.6288", "79.4192"); // Pass the ID of the map element

                // // Example: Add marker
                // await mapModule.InvokeVoidAsync("addMarker", map, 51.5, -0.09);

                // // Example: Get markers
                // var markers = await mapModule.InvokeAsync<IEnumerable<location>>("getMarkers", map);
                // foreach (var marker in markers)
                // {
                //     Console.WriteLine($"Latitude: {marker.latitude}, Longitude: {marker.longitude}");
                // }
            }
        }

        /// <summary>
        /// Adds nearby events to the map and updates the filteredItems collection based on the specified centerPoint.
        /// </summary>
        /// <param name="centerPoint">The location around which nearby events are to be determined.</param>
        private async Task AddEventMarkers(EventLocation centerPoint)
        {
            
            // Clear the previously filtered items
            filteredItems.Clear();

            // Iterate through each item in the event list
            foreach (var item in eventList.Items)
            {
                // Calculate the distance between the center point and the current event's location
                double distance = Utlities.CalculateDistance(centerPoint.Latitude, centerPoint.Longitude, item.Location.Latitude, item.Location.Longitude);

                // Check if the distance is within the specified radius (10,000 meters in this case)
                if (distance <= 10000)
                {
                    // Add a marker for the event's location on the map
                    await JSRuntime.InvokeVoidAsync("mapInterop.addMarker", map, item.Location.Latitude, item.Location.Longitude);

                    // Add the event to the filteredItems collection
                    filteredItems.Add(item);
                }
            }

            // Notify Blazor that the state has changed, triggering a re-render of the component
            await InvokeAsync(StateHasChanged);
        }

        private async void ShowNearByEvents()
        {
            clientLocation = await JSRuntime.InvokeAsync<EventLocation>("mapInterop.getCurrentLocation");

            if (clientLocation != null)
            {
                await JSRuntime.InvokeVoidAsync("mapInterop.addMarker", map, clientLocation.Latitude, clientLocation.Longitude);
                await AddEventMarkers(new EventLocation { Latitude = clientLocation.Latitude, Longitude = clientLocation.Longitude });
            }
           
        }

         public async Task ShowNearByEvents(EventLocation eventLocation)
        {
            Console.WriteLine(eventLocation.Name);
            if (eventLocation != null)
            {
                await JSRuntime.InvokeVoidAsync("mapInterop.addMarker", map, eventLocation.Latitude, eventLocation.Longitude);
                await AddEventMarkers(new EventLocation { Latitude = eventLocation.Latitude, Longitude = eventLocation.Longitude });
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await FetchBlogs(new EventFilters());

            if (eventList != null && eventList.Items != null && eventList.Items.Any())
            {

                eventList.Items.ForEach(x => eventLocations.Add(new EventLocation { Name = x.Name, Latitude = x.Location.Latitude, Longitude = x.Location.Longitude }));
            }


        }

        // Method to fetch blogs on filters (tags, division & disciplines) change
        public async Task FetchBlogs(EventFilters eventFilters)
        {
            eventList = await eventService.GetAllEvents(eventFilters);
            await InvokeAsync(StateHasChanged);
        }


    }
}
