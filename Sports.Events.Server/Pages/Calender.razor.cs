using Microsoft.AspNetCore.Components;
using Sports.Events.Server.Data;
using Sports.Events.Server.Models;
using Sports.Events.Server.Services;
using Syncfusion.Blazor.HeatMap.Internal;
using Syncfusion.Blazor.Schedule;
using Syncfusion.Blazor.Navigations;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Sports.Events.Server.Pages
{
    public partial class Calender
    {
        // Injected dependency for accessing blog page data
        [Inject]
        IEventService eventService { get; set; }

        private Event eventList { get; set; } // Property to store the retrieved blog data

        private NavigationManager Navigation { get; set; }
        SfSchedule<AppointmentData> ScheduleRef;
        DateTime CurrentDate = new DateTime(2020, 2, 14);
        List<AppointmentData> DataSource = new List<AppointmentData>();
        //{
        //    new AppointmentData { Id = 1, Subject = "Paris", StartTime = new DateTime(2020, 2, 13, 10, 0, 0) , EndTime = new DateTime(2020, 2, 13, 12, 0, 0) },
        //    new AppointmentData { Id = 2, Subject = "Germany", StartTime = new DateTime(2020, 2, 15, 10, 0, 0) , EndTime = new DateTime(2020, 2, 15, 12, 0, 0) }
        //};
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

            // DataSource.AddRange(eventList.Items.Select(x => new AppointmentData { Id = 1, Subject = x.Name, StartTime = x.StartTime, EndTime = x.EndTime }) );

            //var data =   eventList.Items.Select(x => new AppointmentData { Id = 1, Subject = x.Name, StartTime = x.StartTime, EndTime = x.EndTime });
            //eventList.Items.SelectMany(x => new AppointmentData { Id = 1, Subject = x.Name, StartTime = x.StartTime, EndTime = x.EndTime })
        }

        public async Task FetchBlogs(EventFilters eventFilters)
        {
            eventList = await eventService.GetAllEvents(eventFilters);
            await InvokeAsync(StateHasChanged);
        }

        public void OnEventClick(EventClickArgs<AppointmentData> args)
        {

        }
        private async Task PopupClose()
        {
            await ScheduleRef.CloseQuickInfoPopupAsync();
        }

        private string GetEventDetails(AppointmentData data)
        {
            return data.StartTime.ToString("dddd dd, MMMM yyyy", CultureInfo.InvariantCulture) + " (" + data.StartTime.ToString("hh:mm tt", CultureInfo.InvariantCulture) + "-" + data.EndTime.ToString("hh:mm tt", CultureInfo.InvariantCulture) + ")";
        }


        //private void OnEventClick(ScheduleEventArgs<AppointmentData> args)
        //{
        //    var eventId = args.Data.Id;
        //    Navigation.NavigateTo($"/event/{eventId}");
        //}
        //public void OnNavigating(NavigatingEventArgs args)
        //{
        //    // Get the current URI from NavigationManager
        //    var currentUri = Navigation.Uri;

        //    if (currentUri.Contains("detailedEventPage"))
        //    {
        //        // Extract event ID from the URI
        //        var eventId = GetQueryParameter(currentUri, "eventId");

        //        // Redirect to the detailed event page with the event ID
        //        Navigation.NavigateTo($"/detailedEventPage/{eventId}");
        //    }
        //}

        //private string GetQueryParameter(string uri, string parameterName)
        //{
        //    var uriBuilder = new UriBuilder(uri);
        //    var query = uriBuilder.Query;
        //    var queryParams = System.Web.HttpUtility.ParseQueryString(query);
        //    return queryParams[parameterName];
        //}

    }
}
