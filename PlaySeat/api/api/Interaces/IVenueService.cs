using api.Models;

namespace api.Interaces
{
    public interface IVenueService
    {
        Task<string> CreateVenueAsync(Venue venue);
        Task<IEnumerable<Venue>> GetAllVenuesAsync();
        Task<Venue?> GetVenueByIdAsync(long id);
        Task<string> UpdateVenueAsync(long id, Venue updatedVenue);
        Task<string> DeleteVenueAsync(long id);
    }
}
