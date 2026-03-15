using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findcas.Domain.Entities
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public bool IsMain { get; set; } 

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;
    }
}