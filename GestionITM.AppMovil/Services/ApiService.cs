using System.Net.Http.Json;
using System.Text.Json;

namespace GestionITM.AppMovil.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        
        // Android Emulator usa 10.0.2.2 para acceder al host local
        // Puerto 8080 es el expuesto por docker-compose
        private const string BaseUrl = "http://10.0.2.2:8080/api/";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            try
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
            catch (HttpRequestException)
            {
                // Sin conexión o servidor caído
                return null;
            }
            catch (Exception)
            {
                // Error inesperado
                return null;
            }
        }
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
