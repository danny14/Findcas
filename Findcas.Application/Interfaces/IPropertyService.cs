using Findcas.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findcas.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<int> CreatePropertyAsync(Property property, IBrowserFile? file = null);
        Task<List<Property>> GetAllPropertiesAsync();

        Task<Property?> GetPropertyByIdAsync(int id);

        Task<IEnumerable<string>> SearchColombiaCitiesAsync(string searchQuery);

    }
}
