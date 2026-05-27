using GestionITM.Domain.Entities;

namespace GestionITM.Domain.Interfaces
{
    public interface IMatriculaRepository
    {
        Task<Matricula> AddAsync(Matricula matricula);
        Task<int> CountMatriculasByCursoAsync(int cursoId);
        Task<IEnumerable<Matricula>> GetByEstudianteIdAsync(int estudianteId);
    }
}