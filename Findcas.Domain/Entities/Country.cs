using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findcas.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [MaxLength(3)]
        public string ShortName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        public int PhoneCode { get; set; }

        public List<State> States { get; set; } = new();
    }
}
