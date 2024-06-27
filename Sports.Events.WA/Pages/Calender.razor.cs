using Microsoft.AspNetCore.Components;
using Sports.Events.WA.Data;
using Sports.Events.WA.Models;
using Sports.Events.WA.Services;
using System.Globalization;
using Syncfusion.Blazor.Schedule;
using Microsoft.JSInterop;

namespace Sports.Events.WA.Pages
{
    /// <summary>
    /// Component for displaying events in a calendar view.
    /// </summary>
    public partial class Calender : ComponentBase
    {
        [Inject]
        IEventService eventService { get; set; }

        /// <summary>
        /// The event list retrieved from the service.
        /// </summary>
        private Event eventList { get; set; }

        /// <summary>
        /// Navigation manager for redirecting to event details.
        /// </summary>
        private NavigationManager Navigation { get; set; }

        /// <summary>
        /// Reference to the Syncfusion schedule component.
        /// </summary>
        SfSchedule<AppointmentData> ScheduleRef;

        /// <summary>
        /// The current date displayed on the calendar.
        /// </summary>
        DateTime CurrentDate = new DateTime(2020, 2, 14);

        /// <summary>
        /// Data source for the calendar appointments.
        /// </summary>
        List<AppointmentData> DataSource = new List<AppointmentData>();

        /// <summary>
        /// Represents an appointment in the calendar.
        /// </summary>
        public class AppointmentData
        {
            public int Id { get; set; }
            public string Subject { get; set; }
            public string Location { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }
            public bool IsAllDay { get; set; }
            public string RecurrenceRule { get; set; }
            public string RecurrenceException { get; set; }
            public Nullable<int> RecurrenceID { get; set; }
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await FetchBlogs(new EventFilters());

            int i = 1;
            foreach (var item in eventList.Items)
            {
                DataSource.Add(new AppointmentData { Id = i, Subject = item.Name, StartTime = item.StartTime, EndTime = item.EndTime, Link = "Event/" + item.Id });
                i++;
            }

            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Fetches event data from the service based on given filters.
        /// </summary>
        /// <param name="eventFilters">Filters to apply to the events.</param>
        public async Task FetchBlogs(EventFilters eventFilters)
        {
            eventList = await eventService.GetAllEvents(eventFilters);
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Handles the click event of a calendar event.
        /// </summary>
        /// <param name="args">Event click arguments.</param>
        public void OnEventClick(EventClickArgs<AppointmentData> args)
        {
            // Handle event click
        }

        /// <summary>
        /// Closes the popup for event details.
        /// </summary>
        private async Task PopupClose()
        {
            await ScheduleRef.CloseQuickInfoPopupAsync();
        }

        /// <summary>
        /// Generates details for the given event data.
        /// </summary>
        /// <param name="data">The appointment data.</param>
        /// <returns>A string representing the event details.</returns>
        private string GetEventDetails(AppointmentData data)
        {
            return data.StartTime.ToString("dddd dd, MMMM yyyy", CultureInfo.InvariantCulture) + " (" + data.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture) + "-" + data.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture) + ")";
        }
    }
}
