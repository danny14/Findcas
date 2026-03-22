using System.ComponentModel.DataAnnotations;

namespace Findcas.Web.DTOs
{
    public class CreateReservationDto
    {
        [Required]
        public int PropertyId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
    }
}
