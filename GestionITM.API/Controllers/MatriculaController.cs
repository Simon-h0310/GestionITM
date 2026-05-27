using GestionITM.Domain.Dtos;
using GestionITM.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionITM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Estudiante")]
    public class MatriculaController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculaController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearMatricula([FromBody] MatriculaCreateDto dto)
        {
            // Extraer el ID del estudiante desde el JWT
            var claimEstudianteId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(claimEstudianteId, out int estudianteId))
            {
                return Unauthorized("Token inválido o estudiante no identificado.");
            }

            try
            {
                var result = await _matriculaService.MatricularEstudianteAsync(estudianteId, dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("mis-matriculas")]
        public async Task<IActionResult> GetMisMatriculas()
        {
            var claimEstudianteId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(claimEstudianteId, out int estudianteId))
            {
                return Unauthorized("Token inválido o estudiante no identificado.");
            }

            var result = await _matriculaService.ObtenerMatriculasEstudianteAsync(estudianteId);
            return Ok(result);
        }
    }
}