using AlimentsUsinages.Web.Models.Entities;
using AlimentsUsinages.Web.Models.ViewModels;

namespace AlimentsUsinages.Web.Repositories
{
    public interface IUsinageRepository
    {
        // CRUD de base
        Task<IEnumerable<Usinage>> GetAllWithIncludesAsync();
        Task<Usinage?> GetByIdWithIncludesAsync(int id);
        Task<IEnumerable<Usinage>> GetByDateAsync(DateTime date);
        Task<bool> ExistsAsync(DateTime date, int idAliment, int? excludeId = null);
        Task<int> AddAsync(Usinage usinage);
        Task UpdateAsync(Usinage usinage);
        Task DeleteAsync(int id);
Commande ECHO activÇe.
        // MÇthodes pour l'interface (Çquivalent de vos mÇthodes VB.NET)
        Task<IEnumerable<UsinageViewModel>> GetUsinageViewModelsAsync();
Commande ECHO activÇe.
        // êquivalent RefreshDataGridViewDetail
        Task<UsinageDetailContainerViewModel> GetUsinageDetailsAsync(
            int idUsinage, 
            int idAliment, 
            int idOrigineFormule, 
            decimal pourcentQuantite, 
            bool showAllIngredients);
Commande ECHO activÇe.
        // êquivalent RefreshDataGridViewAll (donnÇes pivot)
        Task<PivotDataViewModel> GetPivotDataAsync(DateTime date);
Commande ECHO activÇe.
        // MÇthodes pour les dÇtails
        Task SaveUsinageDetailsAsync(int usinageId, IEnumerable<UsinageDetailViewModel> details);
        Task ResetUsinageDetailsAsync(int usinageId);
Commande ECHO activÇe.
        // êquivalent getIdAliment
        Task<int?> GetAlimentIdByUsinageAsync(int usinageId);
Commande ECHO activÇe.
        // Statistiques
        Task<decimal> GetTotalQuantiteByDateAsync(DateTime date);
    }
}
