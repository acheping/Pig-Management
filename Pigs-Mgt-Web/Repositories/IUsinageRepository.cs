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
Commande ECHO activ�e.
        // M�thodes pour l'interface (�quivalent de vos m�thodes VB.NET)
        Task<IEnumerable<UsinageViewModel>> GetUsinageViewModelsAsync();
Commande ECHO activ�e.
        // �quivalent RefreshDataGridViewDetail
        Task<UsinageDetailContainerViewModel> GetUsinageDetailsAsync(
            int idUsinage, 
            int idAliment, 
            int idOrigineFormule, 
            decimal pourcentQuantite, 
            bool showAllIngredients);
Commande ECHO activ�e.
        // �quivalent RefreshDataGridViewAll (donn�es pivot)
        Task<PivotDataViewModel> GetPivotDataAsync(DateTime date);
Commande ECHO activ�e.
        // M�thodes pour les d�tails
        Task SaveUsinageDetailsAsync(int usinageId, IEnumerable<UsinageDetailViewModel> details);
        Task ResetUsinageDetailsAsync(int usinageId);
Commande ECHO activ�e.
        // �quivalent getIdAliment
        Task<int?> GetAlimentIdByUsinageAsync(int usinageId);
Commande ECHO activ�e.
        // Statistiques
        Task<decimal> GetTotalQuantiteByDateAsync(DateTime date);
    }
}
