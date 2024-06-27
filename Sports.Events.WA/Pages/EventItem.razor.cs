using Darnton.Blazor.DeviceInterop.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Sports.Events.WA.Models;
using Sports.Events.WA.Services;
using Syncfusion.Blazor;

namespace Sports.Events.WA.Pages
{
    /// <summary>
    /// Component responsible for displaying details of a specific event.
    /// </summary>
    public partial class EventItem : ComponentBase
    {
        /// <summary>
        /// Gets or sets the ID of the event to display.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Injected service for accessing event data.
        /// </summary>
        [Inject]
        IEventService eventService { get; set; }

        /// <summary>
        /// Injected NavigationManager for navigation operations.
        /// </summary>
        [Inject]
        private NavigationManager Navigation { get; set; }

        /// <summary>
        /// Holds the list of all events.
        /// </summary>
        private Event events { get; set; }

        /// <summary>
        /// Holds the details of the specific event.
        /// </summary>
        private Items eventDetail { get; set; }

        /// <summary>
        /// Injected JSRuntime for invoking JavaScript functions.
        /// </summary>
        [Inject]
        private IJSRuntime JS { get; set; }

        /// <summary>
        /// Injected IConfiguration for accessing application settings.
        /// </summary>
        [Inject]
        private IConfiguration _configuration { get; set; }

        /// <summary>
        /// Injected GeolocationService for accessing geolocation information.
        /// </summary>
        [Inject]
        private IGeolocationService GeolocationService { get; set; }

        /// <summary>
        /// Holds the location of the event.
        /// </summary>
        private EventLocation eventLocation { get; set; }

        /// <summary>
        /// Instance of the map.
        /// </summary>
        private object mapInstance;

        /// <summary>
        /// Injected service for interacting with Azure Maps JavaScript functions.
        /// </summary>
        [Inject]
        IAzureMapJs azureMapJs { get; set; }

        /// <summary>
        /// Method called when the component is initialized asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            if (Id != null)
            {
                events = await eventService.GetAllEvents(new());

                if (events != null)
                {
                    eventDetail = events.Items.FirstOrDefault(x => x.Id == Id) ?? new();
                    eventLocation = eventDetail.Location;
                    eventLocation.Name = eventDetail.Name;

                    await azureMapJs.AddMarker(eventLocation.Longitude, eventLocation.Latitude);
                }
            }

            if (Id == null || eventDetail.Id == null)
            {
                Navigation.NavigateTo("/EventList");
            }
        }

        /// <summary>
        /// Method called after the component has been rendered for the first time.
        /// </summary>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await azureMapJs.SetDotNetObj(DotNetObjectReference.Create(this));
                mapInstance = await azureMapJs.InitMap("map");
            }
        }

        /// <summary>
        /// Method to navigate to the join event page.
        /// </summary>
        private async Task JoinEvent()
        {
            Navigation.NavigateTo("/join");
        }

    }
}
