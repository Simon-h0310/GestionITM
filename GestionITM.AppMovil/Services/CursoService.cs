using System.Net.Http.Json;
using System.Text.Json;

namespace GestionITM.AppMovil.Services
{
    public class CursoService
    {
        private readonly HttpClient _httpClient;

        public CursoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<CursoDto>?> GetCursosPaginadosAsync(int page, int pageSize)
        {
            var response = await _httpClient.GetAsync($"curso/paginado?pageNumber={page}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<PagedResult<CursoDto>>(content, options);
            }
            return null;
        }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class CursoDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Creditos { get; set; }
        public int CuposDisponibles { get; set; }
    }
}