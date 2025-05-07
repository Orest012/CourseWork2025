using api.DTO;
using api.Models;

namespace api.Interaces
{
    public interface IEventService
    {
        Task<List<EventView>> GetEvents();
        Task<SportEvent> GetEvent(long id);
        Task<string> CreateEvent(CreateEventDto sportEvent);
        Task<string> UpdateEvent(CreateEventDto sportEvent);
        Task<string> DaleteEvent(int id);
        Task<SportEventView?> GetSportEventView(long id);

    }
}
