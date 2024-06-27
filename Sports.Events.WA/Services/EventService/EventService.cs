using Sports.Events.WA.Data;
using Sports.Events.WA.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace Sports.Events.WA.Services
{
    /// <summary>
    /// Service for retrieving events data.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making requests.</param>
        public EventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieves all events based on the provided filters.
        /// </summary>
        /// <param name="eventfilters">Filters to apply to the events.</param>
        /// <returns>An instance of <see cref="Event"/> containing the filtered events.</returns>
        public async Task<Event> GetAllEvents(EventFilters eventfilters)
        {
            Event events;

            // Center point coordinates for Tirupati
            double centerLatitude = 13.6288;
            double centerLongitude = 79.4192;

            try
            {
                // Retrieve events from the API
                events = await _httpClient.GetFromJsonAsync<Event>("https://api.search.sportcloud.de/api/event") ?? new Event();

                // Add random tags, divisions, disciplines, types, and generate random locations for each event
                foreach (var item in events.Items)
                {
                    var min = 0;
                    var max = EventTags.Tags.Count;
                    Random rand = new Random();
                    item.Tags.AddRange(EventTags.Tags.ElementAt(rand.Next(min, max)));
                    item.Divisions.AddRange(EventTags.Tags.ElementAt(rand.Next(min, max)));
                    item.Disciplines.AddRange(EventTags.Tags.ElementAt(rand.Next(min, max)));
                    item.Types.AddRange(EventTags.Tags.ElementAt(rand.Next(min, max)));
                    item.Location = Utlities.GenerateRandomLocation(centerLatitude, centerLongitude, 300);
                }

                // Apply filters to the events
                if (eventfilters.TagIds != null && eventfilters.TagIds.Any())
                {
                    events.Items = events.Items.Where(x => eventfilters.TagIds.Intersect(x.Tags).Any()).ToList();
                }

                if (!string.IsNullOrEmpty(eventfilters.DivisionsIds))
                {
                    events.Items = events.Items.Where(x => x.Divisions.Contains(eventfilters.DivisionsIds.Trim())).ToList();
                }

                if (eventfilters.DisciplinesIds != null && eventfilters.DisciplinesIds.Any())
                {
                    events.Items = events.Items.Where(x => eventfilters.DisciplinesIds.Intersect(x.Disciplines).Any()).ToList();
                }

                if (eventfilters.EventIds != null && eventfilters.EventIds.Any())
                {
                    events.Items = events.Items.Where(x => eventfilters.EventIds.Contains(x.Id)).ToList();
                }

                if (!string.IsNullOrEmpty(eventfilters.Keyword))
                {
                    events.Items = events.Items.Where(x => x.Name.Contains(eventfilters.Keyword.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Set the total count of events after filtering
                events.Maximumcount = events.Items.Count;

                // Paginate the events based on page number and page size
                if (eventfilters.Page > 0 && eventfilters.PageSize > 0)
                {
                    events.Items = events.Items.Skip((eventfilters.Page - 1) * eventfilters.PageSize).Take(eventfilters.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // In case of exception, initialize events with an empty instance
                events = new Event();
            }

            return events;
        }
    }
}
