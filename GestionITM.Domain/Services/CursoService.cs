using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Domain.Services
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;
        private readonly IMatriculaRepository _matriculaRepository;

        // Suponemos cupo máximo = 30 por defecto.
        private const int CUPO_MAXIMO_CURSO = 30; 

        public CursoService(ICursoRepository cursoRepository, IMatriculaRepository matriculaRepository)
        {
            _cursoRepository = cursoRepository;
            _matriculaRepository = matriculaRepository;
        }

        public async Task<PagedResult<CursoDto>> GetCursosPaginadosAsync(int pageNumber, int pageSize)
        {
            var query = _cursoRepository.QueryAll();
            var totalRecords = await query.CountAsync();

            var cursos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = new List<CursoDto>();
            foreach(var c in cursos)
            {
                var matriculados = await _matriculaRepository.CountMatriculasByCursoAsync(c.Id);
                dtos.Add(new CursoDto
                {
                    Id = c.Id,
                    Codigo = c.Codigo,
                    Nombre = c.Nombre,
                    Creditos = c.Creditos,
                    CuposDisponibles = CUPO_MAXIMO_CURSO - matriculados
                });
            }

            return new PagedResult<CursoDto>
            {
                Items = dtos,
                TotalRegistros = totalRecords,
                PaginaActual = pageNumber,
                RegistrosPorPagina = pageSize,
                TotalPaginas = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }
    }
}