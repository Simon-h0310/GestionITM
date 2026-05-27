using GestionITM.Domain.Entities;
using GestionITM.Domain.Interfaces;
using GestionITM.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GestionITM.Infrastructure.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly ApplicationDbContext _context;

        public MatriculaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Matricula> AddAsync(Matricula matricula)
        {
            await _context.Matriculas.AddAsync(matricula);
            await _context.SaveChangesAsync();
            return matricula;
        }

        public async Task<int> CountMatriculasByCursoAsync(int cursoId)
        {
            return await _context.Matriculas.CountAsync(m => m.CursoId == cursoId && m.Estado == "Activa");
        }

        public async Task<IEnumerable<Matricula>> GetByEstudianteIdAsync(int estudianteId)
        {
            return await _context.Matriculas.Where(m => m.EstudianteId == estudianteId).ToListAsync();
        }
    }
}