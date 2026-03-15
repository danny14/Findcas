using Findcas.Application.Interfaces;
using Findcas.Domain.Entities;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace Findcas.Web.Client.Services
{
    public class PropertyHttpClient : IPropertyService
    {
        private readonly HttpClient _httpClient;

        public PropertyHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            return await _httpClient.GetFromJsonAsync<List<Property>>("api/properties") ?? new();
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
    }
}