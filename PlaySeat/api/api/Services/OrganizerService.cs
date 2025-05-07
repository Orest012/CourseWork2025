using api.Data;
using api.Interaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace api.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly Data.ApplicationDbContext _context;

        public OrganizerService(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateOrganizerAsync(Organizer organizer)
        {
            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();
            return "Organizer profile created";
        }

        public async Task<Organizer?> GetOrganizerByIdAsync(long id)
        {
            return await _context.Organizers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.OrganizerId == id);
        }

        public async Task<string> VerifyOrganizerAsync(long id)
        {
            var organizer = await _context.Organizers.FindAsync(id);
            if (organizer == null) return "Organizer not found";

            organizer.Verified = true;
            await _context.SaveChangesAsync();
            return "Organizer verified";
        }
    }
}
