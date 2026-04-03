using Findcas.Application.Interfaces;
using Findcas.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Findcas.Web.Client.Services
{
    public class PropertyHttpClient : IPropertyService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navManager;

        public PropertyHttpClient(HttpClient httpClient, NavigationManager navManager)
        {
            _httpClient = httpClient;
            _navManager = navManager;
        }

        public async Task<int> CreatePropertyAsync(Property property, IBrowserFile? file)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(property.Name ?? ""), "Name");
            content.Add(new StringContent(property.Description ?? ""), "Description");
            content.Add(new StringContent(property.PricePerNight.ToString()), "PricePerNight");
            content.Add(new StringContent(property.City ?? ""), "City");
            content.Add(new StringContent(property.Department ?? ""), "Department");
            content.Add(new StringContent(property.Bedrooms.ToString()), "Bedrooms");
            content.Add(new StringContent(property.Bathrooms.ToString()), "Bathrooms");
            content.Add(new StringContent(property.MaxGuests.ToString()), "MaxGuests");

            if (file != null)
            {

                var maxAllowedSize = 1024 * 1024 * 10;
                var fileStream = file.OpenReadStream(maxAllowedSize);

                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                content.Add(fileContent, "ImageFile", file.Name);
            }

            var response = await _httpClient.PostAsync("api/properties", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al crear finca: {error}");
        }

        public async Task<List<Property>> GetAllPropertiesAsync()
        {
            try
            {

                var absoluteUrl = _navManager.ToAbsoluteUri("api/properties").ToString();
                var jsonString = await _httpClient.GetStringAsync(absoluteUrl);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                };

                var result = JsonSerializer.Deserialize<List<Property>>(jsonString, options);
                return result ?? new List<Property>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR DE TRADUCCIÓN JSON: {ex.Message}");
                return new List<Property>();
            }
        }

        public async Task<Property?> GetPropertyByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Property>($"api/properties/{id}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<IEnumerable<string>> SearchColombiaCitiesAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<string>();
            }

            // Hacemos la petición a la API del backend
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<string>>($"api/properties/cities/search?searchQuery={searchQuery}");

            return result ?? new List<string>();
        }
    }
}