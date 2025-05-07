using api.Data;
using api.DTO;
using api.Interaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class EventService : IEventService
    {
        readonly Data.ApplicationDbContext _appDbContext;

        public EventService(Data.ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<SportEvent> GetEvent(long id)
        {
            var res = await _appDbContext.SportEvents
                //.Include(e => e.Organizer)
                //.Include(e => e.Venue)
                .FirstOrDefaultAsync(u => u.SportEventsId == id);

            return res;
        }

        public async Task<SportEventView?> GetSportEventView(long id)
        {
            var sportEvent = await _appDbContext.SportEvents
                .Include(e => e.Organizer)
                .Include(e => e.Venue)
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.SportEventsId == id);

            if (sportEvent == null) return null;

            return new SportEventView
            {
                ImageUrl = sportEvent.Venue?.Imageurl,
                SportEventsId = sportEvent.SportEventsId,
                Title = sportEvent.Title,
                Description = sportEvent.Description,
                EventDate = sportEvent.EventDate,
                SportType = sportEvent.SportType,
                VenueName = sportEvent.Venue?.Name,
                VenueAddress = sportEvent.Venue?.Address,
                VenueCapacity = sportEvent.Venue?.Capacity,
                VenueCity = sportEvent.Venue?.City,
                OrganizerName = sportEvent.Organizer?.OrganizationName,
                OrganizerContact = sportEvent.Organizer?.ContactInfo,
                OrganizerVerified = sportEvent.Organizer?.Verified,
                TotalTickets = sportEvent.Tickets.Count,
                TicketsSold = sportEvent.Tickets.Count(t => t.IsSold == true),
                AveragePrice = sportEvent.Tickets.Any()
                    ? sportEvent.Tickets.Average(t => t.Price ?? 0)
                    : null
            };
        }


        public async Task<string> CreateEvent(CreateEventDto sportEvent)
        {
            var venue = await _appDbContext.Venues
                .FirstOrDefaultAsync(v => v.Name == sportEvent.VenueName);

            if (venue == null)
                return "Venue not found.";

            if (!string.IsNullOrEmpty(sportEvent.ImageUrl))
            {
                venue.Imageurl = sportEvent.ImageUrl;
                _appDbContext.Venues.Update(venue);
            }

            var user = await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.Name == sportEvent.OrganizerName);

            if (user == null)
                return "User not found.";

            var organizer = await _appDbContext.Organizers
                .FirstOrDefaultAsync(o => o.UserId == user.UserId);

            if (organizer == null)
                return "Organizer not found.";

            int maxId = 1;
            if (await _appDbContext.SportEvents.AnyAsync())
            {
                maxId = (int)(await _appDbContext.SportEvents.MaxAsync(e => (long)e.SportEventsId)) + 1;
            }

            var newEvent = new SportEvent
            {
                SportEventsId = maxId,
                Title = sportEvent.Title,
                Description = sportEvent.Description,
                EventDate = sportEvent.EventDate,
                VenueId = venue.VenueId,
                OrganizerId = organizer.OrganizerId,
                SportType = sportEvent.SportType
            };

            _appDbContext.SportEvents.Add(newEvent);
            await _appDbContext.SaveChangesAsync();

            return "New event was created.";
        }



       public async Task<string> DeleteEvent(int id)
{
    // Використовуємо асинхронний метод для отримання події
    var sportEvent = await _appDbContext.SportEvents.FirstOrDefaultAsync(u => u.SportEventsId == id);
    
    // Перевірка на null
    if (sportEvent == null)
    {
        return "Event not found";  // Змінено повідомлення для зрозумілості
    }
    
    // Видаляємо подію
    _appDbContext.SportEvents.Remove(sportEvent);
    
    // Зберігаємо зміни в базі даних
    await _appDbContext.SaveChangesAsync();

    return "Event was deleted successfully";  // Покращено повідомлення
}



        public async Task<List<EventView>> GetEvents()
        {
            return await _appDbContext.SportEvents
                .Include(e => e.Venue)
                .Include(e => e.Organizer)
                .Select(e => new EventView
                {
                    SportEventsId = e.SportEventsId,
                    Title = e.Title,
                    Description = e.Description,
                    EventDate = e.EventDate,
                    SportType = e.SportType,
                    VenueId = e.VenueId,
                    City = e.Venue != null ? e.Venue.City : null,
                    OrganizerId = e.OrganizerId,
                    VenueName = e.Venue.Name,
                    ImageUrl = e.Venue.Imageurl,
                    Organizer = e.Organizer != null ? e.Organizer.OrganizationName : null
                })
                .ToListAsync();
        }


        public async Task<string> UpdateEvent(CreateEventDto sportEvent)
        {
            var existingEvent = await _appDbContext.SportEvents
                .FirstOrDefaultAsync(e => e.SportEventsId == sportEvent.SportEventsId);

            if (existingEvent == null)
            {
                return "Event not found.";
            }

            var venue = await _appDbContext.Venues
                .FirstOrDefaultAsync(v => v.Name == sportEvent.VenueName);

            if (venue == null)
            {
                return "Venue not found.";
            }

            var user = await _appDbContext.Users
                .FirstOrDefaultAsync(u => u.Name == sportEvent.OrganizerName);

            if (user == null)
            {
                return "User not found.";
            }

            var organizer = await _appDbContext.Organizers
                .FirstOrDefaultAsync(o => o.UserId == user.UserId);

            if (organizer == null)
            {
                return "Organizer not found.";
            }

            existingEvent.Title = sportEvent.Title;
            existingEvent.Description = sportEvent.Description;
            existingEvent.EventDate = sportEvent.EventDate;
            existingEvent.VenueId = venue.VenueId; 
            existingEvent.OrganizerId = organizer.OrganizerId; 
            existingEvent.SportType = sportEvent.SportType;

            await _appDbContext.SaveChangesAsync();

            return "Event updated successfully.";
        }

        public Task<string> DaleteEvent(int id)
        {
            throw new NotImplementedException();
        }
    }
}
