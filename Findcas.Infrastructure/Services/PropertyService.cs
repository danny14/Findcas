using Findcas.Application.Interfaces;
using Findcas.Domain.Entities;
using Findcas.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Findcas.Infrastructure.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePropertyAsync(Property property, IBrowserFile? file = null )
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();
            return property.Id;
        }

        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Amenities)
                .ToListAsync();
        }

        public async Task<Property?> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Amenities)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<string>> SearchColombiaCitiesAsync(string searchQuery)
        {

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<string>();
            }

            return await _context.Cities
                .Include(c => c.State)
                .Where(c => c.State.CountryId == 47 && c.Name.Contains(searchQuery))
                .Take(10)
                .Select(c => $"{c.Name}, {c.State.Name}")
                .ToListAsync();
        }
    }
}
