using System;
using System.ComponentModel.DataAnnotations;

namespace Findcas.Web.Client.Models
{
    public class CreateReservationDto
    {
        [Required]
        public int PropertyId { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

        [Required]
        public decimal TotalPrice { get; set; }
    }

    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public PropertyDto? Property { get; set; }
    }

    public class PropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    public class BookedDateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
