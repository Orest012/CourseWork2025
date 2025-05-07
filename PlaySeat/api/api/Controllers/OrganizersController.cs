using api.Interaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizersController : ControllerBase
    {
        private readonly IOrganizerService _organizerService;

        public OrganizersController(IOrganizerService organizerService)
        {
            _organizerService = organizerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganizer([FromBody] Organizer organizer)
        {
            var result = await _organizerService.CreateOrganizerAsync(organizer);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizer(long id)
        {
            var organizer = await _organizerService.GetOrganizerByIdAsync(id);
            return organizer == null ? NotFound("Organizer not found") : Ok(organizer);
        }

        [HttpPost("verify/{id}")]
        public async Task<IActionResult> VerifyOrganizer(long id)
        {
            var result = await _organizerService.VerifyOrganizerAsync(id);
            return result == "Organizer not found" ? NotFound(result) : Ok(result);
        }
    }

}
