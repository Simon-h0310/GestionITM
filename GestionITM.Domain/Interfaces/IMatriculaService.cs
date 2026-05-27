using GestionITM.Domain.Dtos;

namespace GestionITM.Domain.Interfaces
{
    public interface IMatriculaService
    {
        Task<MatriculaDto> MatricularEstudianteAsync(int estudianteId, MatriculaCreateDto dto);
        Task<IEnumerable<MatriculaDto>> ObtenerMatriculasEstudianteAsync(int estudianteId);
    }
}