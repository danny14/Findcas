using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Findcas.Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Dinero siempre en decimal (evita errores de redondeo)
        public decimal PricePerNight { get; set; }

        public int MaxGuests { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }

        // Ubicación (para el mapa más adelante)
        public string City { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty; // Ej: Quindío
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsActive { get; set; } = true; // Para ocultarla sin borrarla

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        [JsonIgnore]
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public List<Amenity> Amenities { get; set; } = new();
    }
}
