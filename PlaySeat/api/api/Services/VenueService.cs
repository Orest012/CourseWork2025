using api.Data;
using api.Interaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace api.Services
{
    public class VenueService : IVenueService
    {
        private readonly Data.ApplicationDbContext _context;

        public VenueService(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateVenueAsync(Venue venue)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            return "Venue created successfully.";
        }

        public async Task<IEnumerable<Venue>> GetAllVenuesAsync()
        {
            return await _context.Venues.ToListAsync();
        }

        public async Task<Venue?> GetVenueByIdAsync(long id)
        {
            return await _context.Venues.FindAsync(id);
        }

        public async Task<string> UpdateVenueAsync(long id, Venue updatedVenue)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return "Venue not found.";

            venue.Name = updatedVenue.Name;
            venue.Address = updatedVenue.Address;
            venue.Capacity = updatedVenue.Capacity;
            venue.City = updatedVenue.City;

            await _context.SaveChangesAsync();
            return "Venue updated successfully.";
        }

        public async Task<string> DeleteVenueAsync(long id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return "Venue not found.";

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return "Venue deleted.";
        }
    }

}
