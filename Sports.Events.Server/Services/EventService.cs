using Sports.Events.Server.Data;
using Sports.Events.Server.Models;
using System.Linq;
using System.Net.Http;

namespace Sports.Events.Server.Services
{

    public class EventService : IEventService
    {
        private readonly HttpClient _httpClient;

        public EventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Event> GetAllEvents(EventFilters eventfilters)
        {

            Event events;
            // Center point coordinates for Tirupati (You can change this to any desired location)
            double centerLatitude = 13.6288; // Example latitude for Tirupati
            double centerLongitude = 79.4192; // Example longitude for Tirupati

            try
            {
                events = await _httpClient.GetFromJsonAsync<Event>("https://api.search.sportcloud.de/api/event") ?? new Event();

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

                if (eventfilters.TagIds != null && eventfilters.TagIds.Any())
                {
                    events.Items = events.Items.Where(x => eventfilters.TagIds.Intersect(x.Tags).Any()).ToList();
                }

                //if (eventfilters.DivisionsIds != null && eventfilters.DivisionsIds.Any())
                //{
                //    events.Items = events.Items.Where(x => eventfilters.DivisionsIds.Intersect(x.Divisions).Any()).ToList();
                //}

                if (!string.IsNullOrEmpty(eventfilters.DivisionsIds))
                {
                    events.Items = events.Items.Where(x => x.Divisions.Contains(eventfilters.DivisionsIds.Trim())).ToList();
                }

                if (eventfilters.DisciplinesIds != null && eventfilters.DisciplinesIds.Any())
                {
                    events.Items = events.Items.Where(x => eventfilters.DisciplinesIds.Intersect(x.Disciplines).Any()).ToList();
                }

                //if (eventfilters != null && eventfilters.TagIds.Any())
                //{
                //    events.Items = events.Items.Where(x => eventfilters.TagIds.Intersect(x.Tags).Any()).ToList();
                //}

                if (!string.IsNullOrEmpty(eventfilters.Keyword))
                {
                    events.Items = events.Items.Where(x => x.Name.Contains(eventfilters.Keyword.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
                }

                events.Maximumcount = events.Items.Count;
                events.Items = events.Items.Skip((eventfilters.Page - 1) * eventfilters.PageSize ).Take(eventfilters.PageSize).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                events = new Event();
            }

            return events;
        }
    }



}

