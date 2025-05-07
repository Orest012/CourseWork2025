using api.Data;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;
        public TicketsController(Data.ApplicationDbContext context) => _context = context;

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetTicketsForEvent(long eventId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.EventId == eventId)
                .ToListAsync();
            return Ok(tickets);
        }

        [HttpPost("buy/{ticketId}/user/{userId}")]
        public async Task<IActionResult> BuyTicket(long ticketId, long userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return BadRequest("User does not exist");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var ticket = await _context.Tickets
                    .Where(t => t.TicketId == ticketId)
                    .FirstOrDefaultAsync();

                if (ticket == null)
                {
                    return NotFound("Ticket not found");
                }

                if (ticket.IsSold == true)
                {
                    return BadRequest("Ticket already sold");
                }

                // Позначаємо квиток як проданий
                ticket.IsSold = true;
                ticket.UserId = userId;
                //ticket.PurchasedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                ticket.PurchasedAt = DateTime.UtcNow;
                _context.Tickets.Update(ticket);

                // Створюємо платіж
                if (ticket.Price == null)
                {
                    return BadRequest("Ticket price is not set.");
                }

                var payment = new Payment
                {
                    UserId = userId,
                    TicketId = ticket.TicketId,
                    //PaymentDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
                    PaymentDate = DateTime.UtcNow,
                    Amount = ticket.Price.Value,
                    PaymentMethod = "Card"
                };

                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Ticket successfully purchased");
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return Ok("Ticket created");
        }

        [HttpGet("MyTickets")]
        public async Task<IActionResult> GetMyTickets()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userIdClaim = _context.Users.FirstOrDefault(u => u.UserId == Convert.ToInt32(userId));
            if (userIdClaim == null)
                return Unauthorized("Користувача не знайдено");


            var tickets = await _context.Tickets
                .Where(t => t.UserId == Convert.ToInt32(userId) && t.IsSold == true)
                .Include(t => t.Event)
                .Select(t => new
                {
                    t.TicketId,
                    t.SeatNumber,
                    t.Section,
                    t.Price,
                    t.PurchasedAt,
                    EventTitle = t.Event.Title,
                    EventDate = t.Event.EventDate,
                    Venue = t.Event.Venue != null ? t.Event.Venue.Name : null
                })
                .ToListAsync();

            return Ok(tickets);
        }
    }

}
