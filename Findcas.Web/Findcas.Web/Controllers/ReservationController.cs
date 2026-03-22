using Findcas.Application.Interfaces;
using Findcas.Domain.Entities;
using Findcas.Web.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Findcas.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Sesión inválida o expirada.");
                }

                var reservation = new Reservation
                {
                    PropertyId = dto.PropertyId,
                    UserId = userId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    TotalPrice = dto.TotalPrice,
                    Status = "Confirmada" 
                };

                await _reservationService.CreateReservationAsync(reservation);

                return Ok(new { Mensaje = "¡Reserva creada con éxito!" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-reservations")]
        public async Task<IActionResult> GetMyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var reservations = await _reservationService.GetUserReservationsAsync(userId);
            return Ok(reservations);
        }

        [HttpGet("property/{propertyId}/booked-dates")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookedDates(int propertyId)
        {
            var reservations = await _reservationService.GetPropertyReservationsAsync(propertyId);

            var bookedDates = reservations.Select(r => new
            {
                StartDate = r.StartDate,
                EndDate = r.EndDate
            }).ToList();

            return Ok(bookedDates);
        }
    }
}
