using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using AlimentsUsinages.Web.Data;
using AlimentsUsinages.Web.Models.Entities;
using AlimentsUsinages.Web.Models.ViewModels;

namespace AlimentsUsinages.Web.Repositories
{
    public class UsinageRepository : IUsinageRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsinageRepository> _logger;

        public UsinageRepository(ApplicationDbContext context, ILogger<UsinageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // êquivalent RefreshDataGridView
        public async Task<IEnumerable<UsinageViewModel>> GetUsinageViewModelsAsync()
        {
            return await _context.Usinages
                .Include(u => u.Aliment)
                    .ThenInclude(a => a.TypeAliment)
                .Include(u => u.OrigineFormule)
                .OrderByDescending(u => u.Date)
                .Select(u => new UsinageViewModel
                {
                    Id = u.Id,
                    Date = u.Date,
                    AlimentName = u.Aliment.Name,
                    OrigineFormuleName = u.OrigineFormule.Name,
                    TypeAlimentName = u.Aliment.TypeAliment.Name,
                    QuantiteVoulue = u.QuantiteVoulue,
                    QuantiteReelle = u.QuantiteReelle,
                    Commentaire = u.Commentaire,
                    IdAliment = u.IdAliment,
                    IdOrigineFormule = u.IdOrigineFormule
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<Usinage>> GetAllWithIncludesAsync()
        {
            return await _context.Usinages
                .Include(u => u.Aliment)
                    .ThenInclude(a => a.TypeAliment)
                .Include(u => u.OrigineFormule)
                .Include(u => u.UsinageDetails)
                    .ThenInclude(ud => ud.Ingredient)
                .OrderByDescending(u => u.Date)
                .ToListAsync();
        }

        public async Task<Usinage?> GetByIdWithIncludesAsync(int id)
        {
            return await _context.Usinages
                .Include(u => u.Aliment)
                    .ThenInclude(a => a.TypeAliment)
                .Include(u => u.OrigineFormule)
                .Include(u => u.UsinageDetails)
                    .ThenInclude(ud => ud.Ingredient)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usinage>> GetByDateAsync(DateTime date)
        {
            return await _context.Usinages
                .Include(u => u.Aliment)
                .Include(u => u.OrigineFormule)
                .Where(u => u.Date.Date == date.Date)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(DateTime date, int idAliment, int? excludeId = null)
        {
            var query = _context.Usinages
                .Where(u => u.Date.Date == date.Date ^
Commande ECHO activÇe.
            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }
Commande ECHO activÇe.
            return await query.AnyAsync();
        }

        public async Task<int> AddAsync(Usinage usinage)
        {
            _context.Usinages.Add(usinage);
            await _context.SaveChangesAsync();
            return usinage.Id;
        }

        public async Task UpdateAsync(Usinage usinage)
        {
            _context.Entry(usinage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usinage = await _context.Usinages.FindAsync(id);
            if (usinage != null)
            {
                _context.Usinages.Remove(usinage);
                await _context.SaveChangesAsync();
            }
        }

        // êquivalent getIdAliment
        public async Task<int?> GetAlimentIdByUsinageAsync(int usinageId)
        {
            var usinage = await _context.Usinages
                .Where(u => u.Id == usinageId)
                .Select(u => u.IdAliment)
                .FirstOrDefaultAsync();
Commande ECHO activÇe.
            return usinage == 0 ? null : usinage;
        }

        // êquivalent ResetDetailFromDatabase
        public async Task ResetUsinageDetailsAsync(int usinageId)
        {
            var details = _context.UsinageDetails.Where(ud => ud.IdUsinage == usinageId);
            _context.UsinageDetails.RemoveRange(details);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalQuantiteByDateAsync(DateTime date)
        {
            return await _context.Usinages
                .Where(u => u.Date.Date == date.Date)
                .SumAsync(u => u.QuantiteVoulue);
        }

        // TODO: ImplÇmenter les mÇthodes complexes
        public async Task<UsinageDetailContainerViewModel> GetUsinageDetailsAsync(int idUsinage, int idAliment, int idOrigineFormule, decimal pourcentQuantite, bool showAllIngredients)
        {
            // TODO: ImplÇmenter la logique complexe Çquivalente Ö RefreshDataGridViewDetail
            throw new NotImplementedException("∑ implÇmenter selon la logique VB.NET");
        }

        public async Task<PivotDataViewModel> GetPivotDataAsync(DateTime date)
        {
            // TODO: ImplÇmenter la logique Çquivalente Ö RefreshDataGridViewAll
            throw new NotImplementedException("∑ implÇmenter selon la logique VB.NET");
        }

        public async Task SaveUsinageDetailsAsync(int usinageId, IEnumerable<UsinageDetailViewModel> details)
        {
            // TODO: ImplÇmenter la logique Çquivalente Ö SaveDataGridViewDetailToDatabase
            throw new NotImplementedException("∑ implÇmenter selon la logique VB.NET");
        }
    }
}
