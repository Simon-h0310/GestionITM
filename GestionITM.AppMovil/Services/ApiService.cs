using System.Net.Http.Json;
using System.Text.Json;

namespace GestionITM.AppMovil.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            var loginData = new { correo = email, contraseña = password };
            var response = await _httpClient.PostAsJsonAsync("auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var authResult = JsonSerializer.Deserialize<AuthResponse>(responseContent, options);
                return authResult?.Token;
            }

            return null;
        }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}