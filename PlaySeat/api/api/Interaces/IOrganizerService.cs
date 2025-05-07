using api.Models;

namespace api.Interaces
{
    public interface IOrganizerService
    {
        Task<string> CreateOrganizerAsync(Organizer organizer);
        Task<Organizer?> GetOrganizerByIdAsync(long id);
        Task<string> VerifyOrganizerAsync(long id);
    }
}
