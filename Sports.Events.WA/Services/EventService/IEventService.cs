using Sports.Events.WA.Data;
using Sports.Events.WA.Models;

namespace Sports.Events.WA.Services
{
    /// <summary>
    /// Interface for accessing event-related data.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Retrieves all events based on the provided filters.
        /// </summary>
        /// <param name="eventfilters">Filters to apply to the event data.</param>
        /// <returns>An object representing the retrieved events.</returns>
        Task<Event> GetAllEvents(EventFilters eventfilters);
    }
}
