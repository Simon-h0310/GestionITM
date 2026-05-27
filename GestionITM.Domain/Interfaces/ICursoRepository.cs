using GestionITM.Domain.Entities;

namespace GestionITM.Domain.Interfaces
{
    public interface ICursoRepository
    {
        Task<IEnumerable<Curso>> ObtenerTodoAsync();
        Task<Curso?> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Curso curso);
        
        // Nivel 5: consulta diferida para paginación (IQueryable)
        IQueryable<Curso> QueryAll();
    }
}
