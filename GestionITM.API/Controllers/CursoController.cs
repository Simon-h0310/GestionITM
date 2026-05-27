using Microsoft.AspNetCore.Mvc;
using GestionITM.Domain.Interfaces;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Dtos;
using GestionITM.Domain.Models;

namespace GestionITM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _repository;
        private readonly ICursoService _cursoService;

        public CursoController(ICursoRepository repository, ICursoService cursoService)
        {
            _repository = repository;
            _cursoService = cursoService;
        }

        // GET: api/curso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
            var cursos = await _repository.ObtenerTodoAsync();
            return Ok(cursos);
        }

        /// <summary>
        /// Obtiene los cursos paginados con información de cupos disponibles
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Registros por página (por defecto 10)</param>
        /// <returns>Lista paginada de cursos con cupos disponibles</returns>
        // GET: api/curso/paginado
        [HttpGet("paginado")]
        public async Task<ActionResult<PagedResult<CursoDto>>> GetCursosPaginado(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var resultado = await _cursoService.GetCursosPaginadosAsync(pageNumber, pageSize);
            return Ok(resultado);
        }

        // GET: api/curso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(int id)
        {
            var curso = await _repository.ObtenerPorIdAsync(id);
            if (curso == null)
            {
                return NotFound(new { message = $"Curso con ID {id} no encontrado." });
            }
            return Ok(curso);
        }

        // POST: api/curso
        [HttpPost]
        public async Task<ActionResult> PostCurso(Curso curso)
        {
            await _repository.AgregarAsync(curso);
            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }
    }
}
