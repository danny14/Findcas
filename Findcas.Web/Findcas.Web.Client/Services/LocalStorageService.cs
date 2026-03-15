using Microsoft.JSInterop;
using System.Text.Json;

namespace Findcas.Web.Client.Services
{
    public class LocalStorageService
    {

        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Guardar el Token
        public async Task SetItemAsync(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        // Leer el Token
        public async Task<string?> GetItemAsync(string key)
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        }

        // Borrar el Token (Para cerrar sesión)
        public async Task RemoveItemAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }

    }
}
