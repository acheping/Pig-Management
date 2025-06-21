using AlimentsUsinages.Web.Models.ViewModels;
using AlimentsUsinages.Web.Models.DTOs;

namespace AlimentsUsinages.Web.Services
{
    public interface IUsinageService
    {
        // MÇthodes principales (Çquivalent des ÇvÇnements de boutons)
        Task<UsinageIndexViewModel> GetIndexViewModelAsync(DateTime? selectedDate = null);
        Task<ApiResponse<int>> CreateUsinageAsync(CreateUsinageViewModel model);
        Task<ApiResponse<bool>> UpdateUsinageAsync(int id, CreateUsinageViewModel model);
        Task<ApiResponse<bool>> DeleteUsinageAsync(int id);
Commande ECHO activÇe.
        // êquivalent de vos mÇthodes de rafraåchissement
        Task<IEnumerable<UsinageViewModel>> GetAllUsinagesAsync();
        Task<UsinageDetailContainerViewModel> GetUsinageDetailsAsync(int usinageId, bool showAllIngredients = false);
        Task<PivotDataViewModel> GetPivotDataAsync(DateTime date);
Commande ECHO activÇe.
        // MÇthodes pour les ComboBox
        Task<ComboBoxDataDto> GetComboBoxDataAsync();
        Task<IEnumerable<AlimentDto>> GetAlimentsByOrigineFormuleAsync(int idOrigineFormule);
Commande ECHO activÇe.
        // OpÇrations sur les dÇtails (Çquivalent dgvDetail)
        Task<ApiResponse<bool>> SaveUsinageDetailsAsync(int usinageId, IEnumerable<UsinageDetailViewModel> details);
        Task<ApiResponse<bool>> ResetUsinageDetailsAsync(int usinageId);
Commande ECHO activÇe.
        // Validation mÇtier
        Task<bool> CanCreateUsinageAsync(DateTime date, int idAliment);
        Task<bool> CanUpdateUsinageAsync(int id, DateTime date, int idAliment);
    }
}
