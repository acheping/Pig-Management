using Microsoft.EntityFrameworkCore;
using AlimentsUsinages.Web.Data;
using AlimentsUsinages.Web.Models.DTOs;

namespace AlimentsUsinages.Web.Repositories
{
    public class AlimentRepository : IAlimentRepository
    {
        private readonly ApplicationDbContext _context;

        public AlimentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // êquivalent LoadComboBoxOriginesFormules
        public async Task<IEnumerable<OrigineFormuleDto>> GetAllOrigineFormulesAsync()
        {
            return await _context.OrigineFormules
                .Select(o => new OrigineFormuleDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Description = o.Description
                })
                .OrderBy(o => o.Id)
                .ToListAsync();
        }

        // êquivalent LoadComboBoxAliments
        public async Task<IEnumerable<AlimentDto>> GetAlimentsByOrigineFormuleAsync(int idOrigineFormule)
        {
            // TODO: ImplÇmenter la requàte avec JOIN sur Formules
            // SELECT DISTINCT A.Id, A.Name FROM Formules F JOIN Aliments A ON F.IdAliment = A.Id WHERE F.IdOrigineFormule = @IdOrigineFormule
            return await _context.Aliments
                .Include(a => a.TypeAliment)
                .Select(a => new AlimentDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    PrixReference = a.PrixReference,
                    TypeAlimentName = a.TypeAliment.Name,
                    AlerteActive = a.AlerteActive,
                    QuantiteMinimale = a.QuantiteMinimale,
                    Description = a.Description
                })
                .OrderBy(a => a.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<AlimentDto>> GetAllAlimentsAsync()
        {
            return await _context.Aliments
                .Include(a => a.TypeAliment)
                .Select(a => new AlimentDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    PrixReference = a.PrixReference,
                    TypeAlimentName = a.TypeAliment.Name,
                    AlerteActive = a.AlerteActive,
                    QuantiteMinimale = a.QuantiteMinimale,
                    Description = a.Description
                })
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients
                .Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    PrixReference = i.PrixReference,
                    AlerteActive = i.AlerteActive,
                    QuantiteMinimale = i.QuantiteMinimale,
                    Description = i.Description
                })
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<AlimentDto?> GetAlimentByIdAsync(int id)
        {
            return await _context.Aliments
                .Include(a => a.TypeAliment)
                .Where(a => a.Id == id)
                .Select(a => new AlimentDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    PrixReference = a.PrixReference,
                    TypeAlimentName = a.TypeAliment.Name,
                    AlerteActive = a.AlerteActive,
                    QuantiteMinimale = a.QuantiteMinimale,
                    Description = a.Description
                })
                .FirstOrDefaultAsync();
        }
    }
}
