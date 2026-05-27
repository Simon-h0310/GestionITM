using GestionITM.Domain.Dtos;
using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;

namespace GestionITM.Domain.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly ICursoRepository _cursoRepository;

        private const int CUPO_MAXIMO_CURSO = 30;

        public MatriculaService(IMatriculaRepository matriculaRepository, ICursoRepository cursoRepository)
        {
            _matriculaRepository = matriculaRepository;
            _cursoRepository = cursoRepository;
        }

        public async Task<MatriculaDto> MatricularEstudianteAsync(int estudianteId, MatriculaCreateDto dto)
        {
            var curso = await _cursoRepository.ObtenerPorIdAsync(dto.CursoId);
            if (curso == null)
            {
                throw new KeyNotFoundException("El curso no existe.");
            }

            var matriculados = await _matriculaRepository.CountMatriculasByCursoAsync(dto.CursoId);
            if (matriculados >= CUPO_MAXIMO_CURSO)
            {
                throw new InvalidOperationException("No hay cupos disponibles para este curso.");
            }

            var matricula = new Matricula
            {
                EstudianteId = estudianteId,
                CursoId = dto.CursoId,
                Periodo = dto.Periodo,
                Estado = "Activa"
            };

            await _matriculaRepository.AddAsync(matricula);

            return new MatriculaDto
            {
                Id = matricula.Id,
                EstudianteId = matricula.EstudianteId,
                CursoId = matricula.CursoId,
                Periodo = matricula.Periodo,
                Estado = matricula.Estado,
                NombreCurso = curso.Nombre
            };
        }

        public async Task<IEnumerable<MatriculaDto>> ObtenerMatriculasEstudianteAsync(int estudianteId)
        {
            var matriculas = await _matriculaRepository.GetByEstudianteIdAsync(estudianteId);
            var dtos = new List<MatriculaDto>();

            foreach(var m in matriculas)
            {
                var curso = await _cursoRepository.ObtenerPorIdAsync(m.CursoId);
                dtos.Add(new MatriculaDto
                {
                    Id = m.Id,
                    EstudianteId = m.EstudianteId,
                    CursoId = m.CursoId,
                    Periodo = m.Periodo,
                    Estado = m.Estado,
                    NombreCurso = curso?.Nombre ?? "Desconocido"
                });
            }

            return dtos;
        }
    }
}