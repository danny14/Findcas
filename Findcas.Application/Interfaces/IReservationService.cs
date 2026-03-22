using Findcas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findcas.Application.Interfaces
{
    public interface IReservationService
    {
       
        Task<bool> CreateReservationAsync(Reservation reservation);

        Task<IEnumerable<Reservation>> GetUserReservationsAsync(string userId);

        Task<IEnumerable<Reservation>> GetPropertyReservationsAsync(int propertyId);
    }
}
