using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findcas.Domain.Entities
{
    public class State
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        public int CountryId { get; set; }
        public Country? Country { get; set; }

        public List<City> Cities { get; set; } = new();
    }
}
