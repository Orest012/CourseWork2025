using api.Interaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenue([FromBody] Venue venue)
        {
            var result = await _venueService.CreateVenueAsync(venue);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVenues()
        {
            var venues = await _venueService.GetAllVenuesAsync();
            return Ok(venues);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVenue(long id)
        {
            var venue = await _venueService.GetVenueByIdAsync(id);
            return venue == null ? NotFound("Venue not found.") : Ok(venue);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVenue(long id, [FromBody] Venue updatedVenue)
        {
            var result = await _venueService.UpdateVenueAsync(id, updatedVenue);
            return result == "Venue not found." ? NotFound(result) : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVenue(long id)
        {
            var result = await _venueService.DeleteVenueAsync(id);
            return result == "Venue not found." ? NotFound(result) : Ok(result);
        }
    }

}
