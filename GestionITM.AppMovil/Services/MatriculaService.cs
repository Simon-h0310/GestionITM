using System.Net.Http.Json;
using System.Text.Json;

namespace GestionITM.AppMovil.Services
{
    public class MatriculaService
    {
        private readonly HttpClient _httpClient;

        public MatriculaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool isSuccess, string message)> CrearMatriculaAsync(int cursoId, string periodo)
        {
            var data = new { cursoId, periodo };
            var response = await _httpClient.PostAsJsonAsync("matricula", data);
            
            if (response.IsSuccessStatusCode)
            {
                return (true, "Matrícula exitosa.");
            }
            
            var content = await response.Content.ReadAsStringAsync();
            try
            {
                // Leer el json de error que manda el backend
                var errorObj = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return (false, errorObj?.Message ?? "Ocurrió un error inesperado.");
            }
            catch
            {
                return (false, "Ocurrió un error inesperado.");
            }
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}