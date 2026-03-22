using Findcas.Web.Client.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Findcas.Web.Client.Services
{
    public class ReservationHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorage;

        public ReservationHttpClient(HttpClient httpClient, LocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<string> CreateReservationAsync(CreateReservationDto dto)
        {
            try
            {

                var token = await _localStorage.GetItemAsync("authToken");


                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.PostAsJsonAsync("api/reservation", dto);

                if (response.IsSuccessStatusCode)
                {
                    return "OK";
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return string.IsNullOrWhiteSpace(errorMessage) ? "Error desconocido al reservar." : errorMessage;
            }
            catch (Exception ex)
            {
                return $"Error de conexión: {ex.Message}";
            }
        }

        public async Task<List<ReservationResponseDto>> GetMyReservationsAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync("authToken");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var reservations = await _httpClient.GetFromJsonAsync<List<ReservationResponseDto>>("api/reservation/my-reservations");

                return reservations ?? new List<ReservationResponseDto>();
            }
            catch
            {
                return new List<ReservationResponseDto>();
            }
        }

        public async Task<List<BookedDateDto>> GetBookedDatesAsync(int propertyId)
        {
            try
            {
                var dates = await _httpClient.GetFromJsonAsync<List<BookedDateDto>>($"api/reservation/property/{propertyId}/booked-dates");
                return dates ?? new List<BookedDateDto>();
            }
            catch
            {
                return new List<BookedDateDto>();
            }
        }
    }
}
