using Findcas.Application.Interfaces;
using Findcas.Domain.Entities;
using Findcas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Findcas.Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReservationAsync(Reservation reservation)
        {
            if (reservation.StartDate >= reservation.EndDate)
            {
                throw new Exception("La fecha de salida debe ser posterior a la de entrada.");
            }

            bool isOverlapping = await _context.Reservations
                .AnyAsync(r => r.PropertyId == reservation.PropertyId &&
                               r.Status != "Cancelada" &&
                               reservation.StartDate < r.EndDate &&
                               reservation.EndDate > r.StartDate);

            if (isOverlapping)
            {
                throw new Exception("La finca ya se encuentra reservada en esas fechas.");
            }

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Reservation>> GetUserReservationsAsync(string userId)
        {
            return await _context.Reservations
                .Include(r => r.Property)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetPropertyReservationsAsync(int propertyId)
        {
            return await _context.Reservations
                .Where(r => r.PropertyId == propertyId && r.Status != "Cancelada")
                .ToListAsync();
        }
    }
}
