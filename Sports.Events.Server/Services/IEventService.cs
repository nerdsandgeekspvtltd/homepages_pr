using Sports.Events.Server.Data;
using Sports.Events.Server.Models;

namespace Sports.Events.Server.Services
{
    public interface IEventService
    {
        Task<Event> GetAllEvents(EventFilters eventfilters);
    }
}
