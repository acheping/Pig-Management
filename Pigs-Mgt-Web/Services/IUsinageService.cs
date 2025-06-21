using AlimentsUsinages.Web.Models.ViewModels;
using AlimentsUsinages.Web.Models.DTOs;

namespace AlimentsUsinages.Web.Services
{
    public interface IUsinageService
    {
        // M�thodes principales (�quivalent des �v�nements de boutons)
        Task<UsinageIndexViewModel> GetIndexViewModelAsync(DateTime? selectedDate = null);
        Task<ApiResponse<int>> CreateUsinageAsync(CreateUsinageViewModel model);
        Task<ApiResponse<bool>> UpdateUsinageAsync(int id, CreateUsinageViewModel model);
        Task<ApiResponse<bool>> DeleteUsinageAsync(int id);
Commande ECHO activ�e.
        // �quivalent de vos m�thodes de rafra�chissement
        Task<IEnumerable<UsinageViewModel>> GetAllUsinagesAsync();
        Task<UsinageDetailContainerViewModel> GetUsinageDetailsAsync(int usinageId, bool showAllIngredients = false);
        Task<PivotDataViewModel> GetPivotDataAsync(DateTime date);
Commande ECHO activ�e.
        // M�thodes pour les ComboBox
        Task<ComboBoxDataDto> GetComboBoxDataAsync();
        Task<IEnumerable<AlimentDto>> GetAlimentsByOrigineFormuleAsync(int idOrigineFormule);
Commande ECHO activ�e.
        // Op�rations sur les d�tails (�quivalent dgvDetail)
        Task<ApiResponse<bool>> SaveUsinageDetailsAsync(int usinageId, IEnumerable<UsinageDetailViewModel> details);
        Task<ApiResponse<bool>> ResetUsinageDetailsAsync(int usinageId);
Commande ECHO activ�e.
        // Validation m�tier
        Task<bool> CanCreateUsinageAsync(DateTime date, int idAliment);
        Task<bool> CanUpdateUsinageAsync(int id, DateTime date, int idAliment);
    }
}
