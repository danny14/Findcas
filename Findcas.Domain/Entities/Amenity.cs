using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Findcas.Domain.Entities
{
    public class Amenity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Property> Properties { get; set; } = new();
    }
}
