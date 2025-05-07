using api.Data;
using api.DTO;
using api.Interaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 


    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ApplicationDbContext _applicationDbContext;
        public EventController(IEventService eventService, ApplicationDbContext applicationDbContext)
        {
            _eventService = eventService;
            _applicationDbContext = applicationDbContext;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetEvents")]
        public async Task<IActionResult> GetEvents()
        {
            var sport_event = await _eventService.GetEvents();
            return Ok(sport_event);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("GetEvent/{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var sport_event = await _eventService.GetSportEventView(id);
            if (sport_event == null)
                return NotFound();
            return Ok(sport_event);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateEventDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var venue = await _applicationDbContext.Venues
                .FirstOrDefaultAsync(v => v.Name == dto.VenueName);
            if (venue == null)
                return NotFound("Місце проведення не знайдено.");

            var user = await _applicationDbContext.Users
                .FirstOrDefaultAsync(u => u.Name == dto.OrganizerName);
            if (user == null)
                return NotFound("Організатора не знайдено.");

            var organizer = await _applicationDbContext.Organizers
                .FirstOrDefaultAsync(o => o.UserId == user.UserId);
            if (organizer == null)
                return NotFound("Організатор не знайдено.");

            // Отримати максимальний ID, якщо база не пуста
            long nextId = 1;
            if (await _applicationDbContext.SportEvents.AnyAsync())
            {
                nextId = await _applicationDbContext.SportEvents.MaxAsync(e => e.SportEventsId) + 1;
            }

            var newEvent = new SportEvent
            {
                SportEventsId = nextId, // Встановлюємо ID вручну
                Title = dto.Title,
                Description = dto.Description,
                EventDate = dto.EventDate,
                SportType = dto.SportType,
                VenueId = venue.VenueId,
                OrganizerId = organizer.OrganizerId
            };

            _applicationDbContext.SportEvents.Add(newEvent);
            await _applicationDbContext.SaveChangesAsync();

            

            return Ok(new { message = "Подію створено успішно", eventId = newEvent.SportEventsId });
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteEvent/{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var sportEvent = await _applicationDbContext.SportEvents
                .FirstOrDefaultAsync(e => e.SportEventsId == id);

            if (sportEvent == null)
                return NotFound("Подію не знайдено");

            _applicationDbContext.SportEvents.Remove(sportEvent);
            await _applicationDbContext.SaveChangesAsync();

            return Ok("Подію успішно видалено");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] CreateEventDto sportEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _eventService.UpdateEvent(sportEvent);
            if (result == "Event not found.")
                return NotFound(result);

            return Ok(result);
        }


    }
}
