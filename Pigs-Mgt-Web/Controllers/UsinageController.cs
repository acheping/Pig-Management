using Microsoft.AspNetCore.Mvc;
using AlimentsUsinages.Web.Services;
using AlimentsUsinages.Web.Models.ViewModels;
using AlimentsUsinages.Web.Models.DTOs;
using System.Globalization;

namespace AlimentsUsinages.Web.Controllers
{
    [Route("Usinage")]
    public class UsinageController : Controller
    {
        private readonly IUsinageService _usinageService;
        private readonly IAlimentService _alimentService;
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<UsinageController> _logger;

        public UsinageController(
            IUsinageService usinageService,
            IAlimentService alimentService,
            IIngredientService ingredientService,
            ILogger<UsinageController> logger)
        {
            _usinageService = usinageService;
            _alimentService = alimentService;
            _ingredientService = ingredientService;
            _logger = logger;
        }

        #region Pages principales

        /// <summary>
        /// Page principale - Équivalent de votre FUsinages_Load
        /// </summary>
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index(DateTime? selectedDate = null)
        {
            try
            {
                var date = selectedDate ?? DateTime.Today;

                var model = new UsinageIndexViewModel
                {
                    Usinages = await _usinageService.GetAllUsinagesAsync(),
                    OrigineFormules = await _alimentService.GetOrigineFormulesAsync(),
                    SelectedDate = date,
                    PivotData = await _usinageService.GetPivotDataAsync(date)
                };

                // Calcul des statistiques
                model.NombreUsinages = model.Usinages.Count();
                model.TotalQuantiteVoulue = model.Usinages.Sum(u => u.QuantiteVoulue);
                model.TotalQuantiteReelle = model.Usinages.Sum(u => u.QuantiteReelle ?? 0);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de la page Usinage");
                TempData["Error"] = "Erreur lors du chargement des données.";
                return View(new UsinageIndexViewModel());
            }
        }

        /// <summary>
        /// Page de création - Équivalent de votre formulaire de saisie
        /// </summary>
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateUsinageViewModel
            {
                Date = DateTime.Today,
                OrigineFormules = await _alimentService.GetOrigineFormulesAsync()
            };

            return View(model);
        }

        /// <summary>
        /// Page d'édition - Équivalent de votre mode édition
        /// </summary>
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var usinage = await _usinageService.GetUsinageByIdAsync(id);
                if (usinage == null)
                {
                    TempData["Error"] = "Usinage introuvable.";
                    return RedirectToAction(nameof(Index));
                }

                var model = new EditUsinageViewModel
                {
                    Id = usinage.Id,
                    Date = usinage.Date,
                    QuantiteVoulue = usinage.QuantiteVoulue,
                    QuantiteReelle = usinage.QuantiteReelle,
                    Commentaire = usinage.Commentaire,
                    IdAliment = usinage.IdAliment,
                    IdOrigineFormule = usinage.IdOrigineFormule,
                    OrigineFormules = await _alimentService.GetOrigineFormulesAsync(),
                    Aliments = await _alimentService.GetAlimentsByOrigineFormuleAsync(usinage.IdOrigineFormule)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de l'usinage {Id}", id);
                TempData["Error"] = "Erreur lors du chargement de l'usinage.";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Actions CRUD - Équivalent de vos boutons

        /// <summary>
        /// Création d'usinage - Équivalent de bAdd_Click
        /// </summary>
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUsinageViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.OrigineFormules = await _alimentService.GetOrigineFormulesAsync();
                    model.Aliments = await _alimentService.GetAlimentsByOrigineFormuleAsync(model.IdOrigineFormule);
                    return View(model);
                }

                var result = await _usinageService.CreateUsinageAsync(model);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    model.OrigineFormules = await _alimentService.GetOrigineFormulesAsync();
                    model.Aliments = await _alimentService.GetAlimentsByOrigineFormuleAsync(model.IdOrigineFormule);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'usinage");
                ModelState.AddModelError("", "Erreur technique lors de la création.");
                model.OrigineFormules = await _alimentService.GetOrigineFormulesAsync();
                return View(model);
            }
        }

        /// <summary>
        /// Modification d'usinage - Équivalent de bEdit_Click
        /// </summary>
        [HttpPost]
        [Route("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditUsinageViewModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    model.OrigineFormules = await _alimentService.GetOrigineFormulesAsync();
                    model.Aliments = await _alimentService.GetAlimentsByOrigineFormuleAsync(model.IdOrigineFormule);
                    return View(model);
                }

                var result = await _usinageService.UpdateUsinageAsync(id, model);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    model.OrigineFormules = await _alimentService.GetOrigineFormulesAsync();
                    model.Aliments = await _alimentService.GetAlimentsByOrigineFormuleAsync(model.IdOrigineFormule);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification de l'usinage {Id}", id);
                ModelState.AddModelError("", "Erreur technique lors de la modification.");
                model.OrigineFormules = await _alimentService.GetOrigineFormulesAsync();
                return View(model);
            }
        }

        /// <summary>
        /// Suppression d'usinage - Équivalent de bDelete_Click
        /// </summary>
        [HttpPost]
        [Route("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _usinageService.DeleteUsinageAsync(id);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                }
                else
                {
                    TempData["Error"] = result.Message;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'usinage {Id}", id);
                TempData["Error"] = "Erreur technique lors de la suppression.";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region API Actions - Équivalent de vos méthodes de rafraîchissement

        /// <summary>
        /// Chargement des aliments par origine - Équivalent de LoadComboBoxAliments
        /// </summary>
        [HttpGet]
        [Route("GetAliments/{idOrigineFormule:int}")]
        public async Task<IActionResult> GetAliments(int idOrigineFormule)
        {
            try
            {
                var aliments = await _alimentService.GetAlimentsByOrigineFormuleAsync(idOrigineFormule);
                return Json(ApiResponse<IEnumerable<AlimentDto>>.SuccessResult(aliments));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des aliments pour l'origine {IdOrigineFormule}", idOrigineFormule);
                return Json(ApiResponse<IEnumerable<AlimentDto>>.ErrorResult("Erreur lors du chargement des aliments."));
            }
        }

        /// <summary>
        /// Données pour la grille principale - Équivalent de RefreshDataGridView
        /// </summary>
        [HttpGet]
        [Route("GetUsinages")]
        public async Task<IActionResult> GetUsinages()
        {
            try
            {
                var usinages = await _usinageService.GetAllUsinagesAsync();
                return Json(ApiResponse<IEnumerable<UsinageViewModel>>.SuccessResult(usinages));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des usinages");
                return Json(ApiResponse<IEnumerable<UsinageViewModel>>.ErrorResult("Erreur lors du chargement des usinages."));
            }
        }

        /// <summary>
        /// Détails d'un usinage - Équivalent de RefreshDataGridViewDetail
        /// </summary>
        [HttpGet]
        [Route("GetDetails/{usinageId:int}")]
        public async Task<IActionResult> GetDetails(int usinageId, bool showAllIngredients = false)
        {
            try
            {
                var details = await _usinageService.GetUsinageDetailsAsync(usinageId, showAllIngredients);
                return Json(ApiResponse<UsinageDetailContainerViewModel>.SuccessResult(details));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des détails de l'usinage {UsinageId}", usinageId);
                return Json(ApiResponse<UsinageDetailContainerViewModel>.ErrorResult("Erreur lors du chargement des détails."));
            }
        }

        /// <summary>
        /// Données pivot par date - Équivalent de RefreshDataGridViewAll
        /// </summary>
        [HttpGet]
        [Route("GetPivotData")]
        public async Task<IActionResult> GetPivotData(string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return Json(ApiResponse<PivotDataViewModel>.ErrorResult("Format de date invalide."));
                }

                var pivotData = await _usinageService.GetPivotDataAsync(parsedDate);
                return Json(ApiResponse<PivotDataViewModel>.SuccessResult(pivotData));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des données pivot pour la date {Date}", date);
                return Json(ApiResponse<PivotDataViewModel>.ErrorResult("Erreur lors du chargement des données pivot."));
            }
        }

        /// <summary>
        /// Sauvegarde des détails - Équivalent de bApplyDetail_Click
        /// </summary>
        [HttpPost]
        [Route("SaveDetails")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDetails([FromBody] SaveDetailsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(CrudResultDto.Error("Données invalides."));
                }

                var result = await _usinageService.SaveUsinageDetailsAsync(request.UsinageId, request.Details);

                if (result.Success)
                {
                    return Json(CrudResultDto.Success(result.Message));
                }
                else
                {
                    return Json(CrudResultDto.Error(result.Message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la sauvegarde des détails de l'usinage {UsinageId}", request?.UsinageId);
                return Json(CrudResultDto.Error("Erreur technique lors de la sauvegarde."));
            }
        }

        /// <summary>
        /// Reset des détails - Équivalent de bResetDetail_Click
        /// </summary>
        [HttpPost]
        [Route("ResetDetails/{usinageId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetDetails(int usinageId)
        {
            try
            {
                var result = await _usinageService.ResetUsinageDetailsAsync(usinageId);

                if (result.Success)
                {
                    return Json(CrudResultDto.Success(result.Message));
                }
                else
                {
                    return Json(CrudResultDto.Error(result.Message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du reset des détails de l'usinage {UsinageId}", usinageId);
                return Json(CrudResultDto.Error("Erreur technique lors du reset."));
            }
        }

        #endregion

        #region Modèles pour les requêtes

        public class SaveDetailsRequest
        {
            public int UsinageId { get; set; }
            public List<UsinageDetailViewModel> Details { get; set; } = new();
        }

        #endregion
    }
}