using Findcas.Web.Client.Models;
using System.Net.Http.Json;

namespace Findcas.Web.Client.Services
{
    public class AuthHttpClient
    {
        private readonly HttpClient _httpClient;

        public AuthHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                return result ?? new AuthResponseDto();
            }

            return new AuthResponseDto { Mensaje = "Error al iniciar sesión. Verifica tus credenciales." };
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                return "OK";
            }

            var error = await response.Content.ReadAsStringAsync();
            return $"Error: {error}";
        }
    }
}
