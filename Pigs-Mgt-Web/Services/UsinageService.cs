using AlimentsUsinages.Web.Models.ViewModels;
using AlimentsUsinages.Web.Models.DTOs;
using AlimentsUsinages.Web.Models.Entities;
using AlimentsUsinages.Web.Repositories;

namespace AlimentsUsinages.Web.Services
{
    public class UsinageService : IUsinageService
    {
        private readonly IUsinageRepository _usinageRepository;
        private readonly IAlimentRepository _alimentRepository;
        private readonly ILogger<UsinageService> _logger;

        public UsinageService(
            IUsinageRepository usinageRepository,
            IAlimentRepository alimentRepository,
            ILogger<UsinageService> logger)
        {
            _usinageRepository = usinageRepository;
            _alimentRepository = alimentRepository;
            _logger = logger;
        }

        // êquivalent FUsinages_Load
        public async Task<UsinageIndexViewModel> GetIndexViewModelAsync(DateTime? selectedDate = null)
        {
            try
            {
                var date = selectedDate ?? DateTime.Today;
Commande ECHO activÇe.
                var usinages = await _usinageRepository.GetUsinageViewModelsAsync();
                var origineFormules = await _alimentRepository.GetAllOrigineFormulesAsync();
                var aliments = await _alimentRepository.GetAllAlimentsAsync();
                var pivotData = await _usinageRepository.GetPivotDataAsync(date);
Commande ECHO activÇe.
                return new UsinageIndexViewModel
                {
                    Usinages = usinages,
                    OrigineFormules = origineFormules,
                    Aliments = aliments,
                    SelectedDate = date,
                    PivotData = pivotData,
                    TotalQuantiteVoulue = usinages.Sum(u => u.QuantiteVoulue),
                    TotalQuantiteReelle = usinages.Sum(u => u.QuantiteReelle ?? 0),
                    NombreUsinages = usinages.Count()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de la vue principale");
                throw;
            }
        }

        // êquivalent bAdd_Click
        public async Task<ApiResponse<int>> CreateUsinageAsync(CreateUsinageViewModel model)
        {
            try
            {
                // VÇrification de la contrainte unique Date/Aliment
                if (await _usinageRepository.ExistsAsync(model.Date, model.IdAliment))
                {
                    return ApiResponse<int>.ErrorResult("Un usinage existe dÇjÖ pour cet aliment Ö cette date.");
                }
Commande ECHO activÇe.
                // Validation mÇtier
                var aliment = await _alimentRepository.GetAlimentByIdAsync(model.IdAliment);
                if (aliment == null)
                {
                    return ApiResponse<int>.ErrorResult("L'aliment sÇlectionnÇ n'existe pas.");
                }
Commande ECHO activÇe.
                var usinage = new Usinage
                {
                    Date = model.Date.Date,
                    QuantiteVoulue = model.QuantiteVoulue,
                    QuantiteReelle = model.QuantiteReelle,
                    Commentaire = model.Commentaire,
                    IdAliment = model.IdAliment,
                    IdOrigineFormule = model.IdOrigineFormule
                };
Commande ECHO activÇe.
                var id = await _usinageRepository.AddAsync(usinage);
Commande ECHO activÇe.
                _logger.LogInformation("Usinage crÇÇ avec succäs - ID: {UsinageId}", id);
                return ApiResponse<int>.SuccessResult(id, "Usinage crÇÇ avec succäs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la crÇation de l'usinage");
                return ApiResponse<int>.ErrorResult("Erreur technique lors de la crÇation.");
            }
        }

        // êquivalent bEdit_Click
        public async Task<ApiResponse<bool>> UpdateUsinageAsync(int id, CreateUsinageViewModel model)
        {
            try
            {
                var existingUsinage = await _usinageRepository.GetByIdWithIncludesAsync(id);
                if (existingUsinage == null)
                {
                    return ApiResponse<bool>.ErrorResult("L'usinage Ö modifier n'existe pas.");
                }
Commande ECHO activÇe.
                // VÇrification de la contrainte unique (en excluant l'usinage actuel)
                if (await _usinageRepository.ExistsAsync(model.Date, model.IdAliment, id))
                {
                    return ApiResponse<bool>.ErrorResult("Un autre usinage existe dÇjÖ pour cet aliment Ö cette date.");
                }
Commande ECHO activÇe.
                existingUsinage.Date = model.Date.Date;
                existingUsinage.QuantiteVoulue = model.QuantiteVoulue;
                existingUsinage.QuantiteReelle = model.QuantiteReelle;
                existingUsinage.Commentaire = model.Commentaire;
                existingUsinage.IdAliment = model.IdAliment;
                existingUsinage.IdOrigineFormule = model.IdOrigineFormule;
Commande ECHO activÇe.
                await _usinageRepository.UpdateAsync(existingUsinage);
Commande ECHO activÇe.
                _logger.LogInformation("Usinage mis Ö jour avec succäs - ID: {UsinageId}", id);
                return ApiResponse<bool>.SuccessResult(true, "Usinage mis Ö jour avec succäs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise Ö jour de l'usinage {UsinageId}", id);
                return ApiResponse<bool>.ErrorResult("Erreur technique lors de la mise Ö jour.");
            }
        }

        // êquivalent bDelete_Click
        public async Task<ApiResponse<bool>> DeleteUsinageAsync(int id)
        {
            try
            {
                var existingUsinage = await _usinageRepository.GetByIdWithIncludesAsync(id);
                if (existingUsinage == null)
                {
                    return ApiResponse<bool>.ErrorResult("L'usinage Ö supprimer n'existe pas.");
                }
Commande ECHO activÇe.
                await _usinageRepository.DeleteAsync(id);
Commande ECHO activÇe.
                _logger.LogInformation("Usinage supprimÇ avec succäs - ID: {UsinageId}", id);
                return ApiResponse<bool>.SuccessResult(true, "Usinage supprimÇ avec succäs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'usinage {UsinageId}", id);
                return ApiResponse<bool>.ErrorResult("Erreur technique lors de la suppression.");
            }
        }

        // êquivalent RefreshDataGridView
        public async Task<IEnumerable<UsinageViewModel>> GetAllUsinagesAsync()
        {
            return await _usinageRepository.GetUsinageViewModelsAsync();
        }

        // êquivalent RefreshDataGridViewDetail
        public async Task<UsinageDetailContainerViewModel> GetUsinageDetailsAsync(int usinageId, bool showAllIngredients = false)
        {
            // TODO: ImplÇmenter la logique complexe
            throw new NotImplementedException("Logique complexe Ö implÇmenter");
        }

        // êquivalent RefreshDataGridViewAll
        public async Task<PivotDataViewModel> GetPivotDataAsync(DateTime date)
        {
            return await _usinageRepository.GetPivotDataAsync(date);
        }

        // êquivalent LoadComboBox methods
        public async Task<ComboBoxDataDto> GetComboBoxDataAsync()
        {
            var aliments = await _alimentRepository.GetAllAlimentsAsync();
            var origineFormules = await _alimentRepository.GetAllOrigineFormulesAsync();
            var ingredients = await _alimentRepository.GetAllIngredientsAsync();
Commande ECHO activÇe.
            return new ComboBoxDataDto
            {
                Aliments = aliments,
                OrigineFormules = origineFormules,
                Ingredients = ingredients
            };
        }

        // êquivalent cbIdOrigineFormule_TextChanged
        public async Task<IEnumerable<AlimentDto>> GetAlimentsByOrigineFormuleAsync(int idOrigineFormule)
        {
            return await _alimentRepository.GetAlimentsByOrigineFormuleAsync(idOrigineFormule);
        }

        // êquivalent bApplyDetail_Click
        public async Task<ApiResponse<bool>> SaveUsinageDetailsAsync(int usinageId, IEnumerable<UsinageDetailViewModel> details)
        {
            try
            {
                await _usinageRepository.SaveUsinageDetailsAsync(usinageId, details);
                return ApiResponse<bool>.SuccessResult(true, "DÇtails sauvegardÇs avec succäs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la sauvegarde des dÇtails");
                return ApiResponse<bool>.ErrorResult("Erreur lors de la sauvegarde");
            }
        }

        // êquivalent bResetDetail_Click
        public async Task<ApiResponse<bool>> ResetUsinageDetailsAsync(int usinageId)
        {
            try
            {
                await _usinageRepository.ResetUsinageDetailsAsync(usinageId);
                return ApiResponse<bool>.SuccessResult(true, "DÇtails rÇinitialisÇs avec succäs");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la rÇinitialisation des dÇtails");
                return ApiResponse<bool>.ErrorResult("Erreur lors de la rÇinitialisation");
            }
        }

        // Validations mÇtier
        public async Task<bool> CanCreateUsinageAsync(DateTime date, int idAliment)
        {
            return !await _usinageRepository.ExistsAsync(date, idAliment);
        }

        public async Task<bool> CanUpdateUsinageAsync(int id, DateTime date, int idAliment)
        {
            return !await _usinageRepository.ExistsAsync(date, idAliment, id);
        }
    }
}
