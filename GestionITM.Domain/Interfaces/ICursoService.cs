using GestionITM.Domain.Dtos;
using GestionITM.Domain.Models;

namespace GestionITM.Domain.Interfaces
{
    public interface ICursoService
    {
        Task<PagedResult<CursoDto>> GetCursosPaginadosAsync(int pageNumber, int pageSize);
    }
}