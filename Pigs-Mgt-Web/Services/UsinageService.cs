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

        // �quivalent FUsinages_Load
        public async Task<UsinageIndexViewModel> GetIndexViewModelAsync(DateTime? selectedDate = null)
        {
            try
            {
                var date = selectedDate ?? DateTime.Today;
Commande ECHO activ�e.
                var usinages = await _usinageRepository.GetUsinageViewModelsAsync();
                var origineFormules = await _alimentRepository.GetAllOrigineFormulesAsync();
                var aliments = await _alimentRepository.GetAllAlimentsAsync();
                var pivotData = await _usinageRepository.GetPivotDataAsync(date);
Commande ECHO activ�e.
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

        // �quivalent bAdd_Click
        public async Task<ApiResponse<int>> CreateUsinageAsync(CreateUsinageViewModel model)
        {
            try
            {
                // V�rification de la contrainte unique Date/Aliment
                if (await _usinageRepository.ExistsAsync(model.Date, model.IdAliment))
                {
                    return ApiResponse<int>.ErrorResult("Un usinage existe d�j� pour cet aliment � cette date.");
                }
Commande ECHO activ�e.
                // Validation m�tier
                var aliment = await _alimentRepository.GetAlimentByIdAsync(model.IdAliment);
                if (aliment == null)
                {
                    return ApiResponse<int>.ErrorResult("L'aliment s�lectionn� n'existe pas.");
                }
Commande ECHO activ�e.
                var usinage = new Usinage
                {
                    Date = model.Date.Date,
                    QuantiteVoulue = model.QuantiteVoulue,
                    QuantiteReelle = model.QuantiteReelle,
                    Commentaire = model.Commentaire,
                    IdAliment = model.IdAliment,
                    IdOrigineFormule = model.IdOrigineFormule
                };
Commande ECHO activ�e.
                var id = await _usinageRepository.AddAsync(usinage);
Commande ECHO activ�e.
                _logger.LogInformation("Usinage cr�� avec succ�s - ID: {UsinageId}", id);
                return ApiResponse<int>.SuccessResult(id, "Usinage cr�� avec succ�s");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la cr�ation de l'usinage");
                return ApiResponse<int>.ErrorResult("Erreur technique lors de la cr�ation.");
            }
        }

        // �quivalent bEdit_Click
        public async Task<ApiResponse<bool>> UpdateUsinageAsync(int id, CreateUsinageViewModel model)
        {
            try
            {
                var existingUsinage = await _usinageRepository.GetByIdWithIncludesAsync(id);
                if (existingUsinage == null)
                {
                    return ApiResponse<bool>.ErrorResult("L'usinage � modifier n'existe pas.");
                }
Commande ECHO activ�e.
                // V�rification de la contrainte unique (en excluant l'usinage actuel)
                if (await _usinageRepository.ExistsAsync(model.Date, model.IdAliment, id))
                {
                    return ApiResponse<bool>.ErrorResult("Un autre usinage existe d�j� pour cet aliment � cette date.");
                }
Commande ECHO activ�e.
                existingUsinage.Date = model.Date.Date;
                existingUsinage.QuantiteVoulue = model.QuantiteVoulue;
                existingUsinage.QuantiteReelle = model.QuantiteReelle;
                existingUsinage.Commentaire = model.Commentaire;
                existingUsinage.IdAliment = model.IdAliment;
                existingUsinage.IdOrigineFormule = model.IdOrigineFormule;
Commande ECHO activ�e.
                await _usinageRepository.UpdateAsync(existingUsinage);
Commande ECHO activ�e.
                _logger.LogInformation("Usinage mis � jour avec succ�s - ID: {UsinageId}", id);
                return ApiResponse<bool>.SuccessResult(true, "Usinage mis � jour avec succ�s");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise � jour de l'usinage {UsinageId}", id);
                return ApiResponse<bool>.ErrorResult("Erreur technique lors de la mise � jour.");
            }
        }

        // �quivalent bDelete_Click
        public async Task<ApiResponse<bool>> DeleteUsinageAsync(int id)
        {
            try
            {
                var existingUsinage = await _usinageRepository.GetByIdWithIncludesAsync(id);
                if (existingUsinage == null)
                {
                    return ApiResponse<bool>.ErrorResult("L'usinage � supprimer n'existe pas.");
                }
Commande ECHO activ�e.
                await _usinageRepository.DeleteAsync(id);
Commande ECHO activ�e.
                _logger.LogInformation("Usinage supprim� avec succ�s - ID: {UsinageId}", id);
                return ApiResponse<bool>.SuccessResult(true, "Usinage supprim� avec succ�s");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'usinage {UsinageId}", id);
                return ApiResponse<bool>.ErrorResult("Erreur technique lors de la suppression.");
            }
        }

        // �quivalent RefreshDataGridView
        public async Task<IEnumerable<UsinageViewModel>> GetAllUsinagesAsync()
        {
            return await _usinageRepository.GetUsinageViewModelsAsync();
        }

        // �quivalent RefreshDataGridViewDetail
        public async Task<UsinageDetailContainerViewModel> GetUsinageDetailsAsync(int usinageId, bool showAllIngredients = false)
        {
            // TODO: Impl�menter la logique complexe
            throw new NotImplementedException("Logique complexe � impl�menter");
        }

        // �quivalent RefreshDataGridViewAll
        public async Task<PivotDataViewModel> GetPivotDataAsync(DateTime date)
        {
            return await _usinageRepository.GetPivotDataAsync(date);
        }

        // �quivalent LoadComboBox methods
        public async Task<ComboBoxDataDto> GetComboBoxDataAsync()
        {
            var aliments = await _alimentRepository.GetAllAlimentsAsync();
            var origineFormules = await _alimentRepository.GetAllOrigineFormulesAsync();
            var ingredients = await _alimentRepository.GetAllIngredientsAsync();
Commande ECHO activ�e.
            return new ComboBoxDataDto
            {
                Aliments = aliments,
                OrigineFormules = origineFormules,
                Ingredients = ingredients
            };
        }

        // �quivalent cbIdOrigineFormule_TextChanged
        public async Task<IEnumerable<AlimentDto>> GetAlimentsByOrigineFormuleAsync(int idOrigineFormule)
        {
            return await _alimentRepository.GetAlimentsByOrigineFormuleAsync(idOrigineFormule);
        }

        // �quivalent bApplyDetail_Click
        public async Task<ApiResponse<bool>> SaveUsinageDetailsAsync(int usinageId, IEnumerable<UsinageDetailViewModel> details)
        {
            try
            {
                await _usinageRepository.SaveUsinageDetailsAsync(usinageId, details);
                return ApiResponse<bool>.SuccessResult(true, "D�tails sauvegard�s avec succ�s");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la sauvegarde des d�tails");
                return ApiResponse<bool>.ErrorResult("Erreur lors de la sauvegarde");
            }
        }

        // �quivalent bResetDetail_Click
        public async Task<ApiResponse<bool>> ResetUsinageDetailsAsync(int usinageId)
        {
            try
            {
                await _usinageRepository.ResetUsinageDetailsAsync(usinageId);
                return ApiResponse<bool>.SuccessResult(true, "D�tails r�initialis�s avec succ�s");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la r�initialisation des d�tails");
                return ApiResponse<bool>.ErrorResult("Erreur lors de la r�initialisation");
            }
        }

        // Validations m�tier
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
