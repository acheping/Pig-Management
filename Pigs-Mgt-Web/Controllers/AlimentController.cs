using Microsoft.AspNetCore.Mvc;
using AlimentsUsinages.Web.Services;
using AlimentsUsinages.Web.Models.ViewModels;
using AlimentsUsinages.Web.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace AlimentsUsinages.Web.Controllers
{
    [Route("Aliment")]
    public class AlimentController : Controller
    {
        private readonly IAlimentService _alimentService;
        private readonly ILogger<AlimentController> _logger;

        public AlimentController(IAlimentService alimentService, ILogger<AlimentController> logger)
        {
            _alimentService = alimentService;
            _logger = logger;
        }

        #region Pages de gestion

        /// <summary>
        /// Page de gestion des aliments
        /// </summary>
        [Route("")]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var aliments = await _alimentService.GetAllAlimentsAsync();
                return View(aliments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des aliments");
                TempData["Error"] = "Erreur lors du chargement des aliments.";
                return View(new List<AlimentDto>());
            }
        }

        /// <summary>
        /// Page de création d'aliment
        /// </summary>
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new CreateAlimentViewModel
                {
                    TypesAliments = await _alimentService.GetTypesAlimentsAsync()
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de la page de création d'aliment");
                TempData["Error"] = "Erreur lors du chargement de la page.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Page d'édition d'aliment
        /// </summary>
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var aliment = await _alimentService.GetAlimentByIdAsync(id);
                if (aliment == null)
                {
                    TempData["Error"] = "Aliment introuvable.";
                    return RedirectToAction(nameof(Index));
                }

                var model = new EditAlimentViewModel
                {
                    Id = aliment.Id,
                    Name = aliment.Name,
                    PrixReference = aliment.PrixReference,
                    QuantiteMinimale = aliment.QuantiteMinimale,
                    AlerteActive = aliment.AlerteActive,
                    Description = aliment.Description,
                    IdTypeAliment = aliment.IdTypeAliment,
                    TypesAliments = await _alimentService.GetTypesAlimentsAsync()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de l'aliment {Id}", id);
                TempData["Error"] = "Erreur lors du chargement de l'aliment.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Page de gestion des origines de formules
        /// </summary>
        [Route("OrigineFormules")]
        public async Task<IActionResult> OrigineFormules()
        {
            try
            {
                var origines = await _alimentService.GetOrigineFormulesAsync();
                return View(origines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des origines de formules");
                TempData["Error"] = "Erreur lors du chargement des origines de formules.";
                return View(new List<OrigineFormuleDto>());
            }
        }

        /// <summary>
        /// Page de gestion des types d'aliments
        /// </summary>
        [Route("TypesAliments")]
        public async Task<IActionResult> TypesAliments()
        {
            try
            {
                var types = await _alimentService.GetTypesAlimentsAsync();
                return View(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des types d'aliments");
                TempData["Error"] = "Erreur lors du chargement des types d'aliments.";
                return View(new List<TypeAlimentDto>());
            }
        }

        #endregion

        #region Actions CRUD pour les Aliments

        /// <summary>
        /// Création d'un aliment
        /// </summary>
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAlimentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.TypesAliments = await _alimentService.GetTypesAlimentsAsync();
                    return View(model);
                }

                var result = await _alimentService.CreateAlimentAsync(model);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    model.TypesAliments = await _alimentService.GetTypesAlimentsAsync();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'aliment");
                ModelState.AddModelError("", "Erreur technique lors de la création.");
                model.TypesAliments = await _alimentService.GetTypesAlimentsAsync();
                return View(model);
            }
        }

        /// <summary>
        /// Modification d'un aliment
        /// </summary>
        [HttpPost]
        [Route("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAlimentViewModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    model.TypesAliments = await _alimentService.GetTypesAlimentsAsync();
                    return View(model);
                }

                var result = await _alimentService.UpdateAlimentAsync(id, model);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    model.TypesAliments = await _alimentService.GetTypesAlimentsAsync();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification de l'aliment {Id}", id);
                ModelState.AddModelError("", "Erreur technique lors de la modification.");
                model.TypesAliments = await _alimentService.GetTypesAlimentsAsync();
                return View(model);
            }
        }

        /// <summary>
        /// Suppression d'un aliment
        /// </summary>
        [HttpPost]
        [Route("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _alimentService.DeleteAlimentAsync(id);

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
                _logger.LogError(ex, "Erreur lors de la suppression de l'aliment {Id}", id);
                TempData["Error"] = "Erreur technique lors de la suppression.";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region API pour les ComboBox - Équivalent de vos LoadComboBox methods

        /// <summary>
        /// API: Tous les aliments - Équivalent de LoadComboBoxAliments
        /// </summary>
        [HttpGet]
        [Route("Api/GetAll")]
        public async Task<IActionResult> GetAllAliments()
        {
            try
            {
                var aliments = await _alimentService.GetAllAlimentsAsync();
                return Json(ApiResponse<IEnumerable<AlimentDto>>.SuccessResult(aliments));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de tous les aliments");
                return Json(ApiResponse<IEnumerable<AlimentDto>>.ErrorResult("Erreur lors du chargement des aliments."));
            }
        }

        /// <summary>
        /// API: Aliments par origine de formule - Équivalent de votre logique de filtrage
        /// </summary>
        [HttpGet]
        [Route("Api/GetByOrigineFormule/{idOrigineFormule:int}")]
        public async Task<IActionResult> GetAlimentsByOrigineFormule(int idOrigineFormule)
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
        /// API: Toutes les origines de formules - Équivalent de LoadComboBoxOriginesFormules
        /// </summary>
        [HttpGet]
        [Route("Api/GetOrigineFormules")]
        public async Task<IActionResult> GetOrigineFormules()
        {
            try
            {
                var origines = await _alimentService.GetOrigineFormulesAsync();
                return Json(ApiResponse<IEnumerable<OrigineFormuleDto>>.SuccessResult(origines));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des origines de formules");
                return Json(ApiResponse<IEnumerable<OrigineFormuleDto>>.ErrorResult("Erreur lors du chargement des origines de formules."));
            }
        }

        /// <summary>
        /// API: Tous les types d'aliments
        /// </summary>
        [HttpGet]
        [Route("Api/GetTypesAliments")]
        public async Task<IActionResult> GetTypesAliments()
        {
            try
            {
                var types = await _alimentService.GetTypesAlimentsAsync();
                return Json(ApiResponse<IEnumerable<TypeAlimentDto>>.SuccessResult(types));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des types d'aliments");
                return Json(ApiResponse<IEnumerable<TypeAlimentDto>>.ErrorResult("Erreur lors du chargement des types d'aliments."));
            }
        }

        /// <summary>
        /// API: Détails d'un aliment spécifique
        /// </summary>
        [HttpGet]
        [Route("Api/GetById/{id:int}")]
        public async Task<IActionResult> GetAlimentById(int id)
        {
            try
            {
                var aliment = await _alimentService.GetAlimentByIdAsync(id);

                if (aliment == null)
                {
                    return Json(ApiResponse<AlimentDto>.ErrorResult("Aliment introuvable."));
                }

                return Json(ApiResponse<AlimentDto>.SuccessResult(aliment));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de l'aliment {Id}", id);
                return Json(ApiResponse<AlimentDto>.ErrorResult("Erreur lors du chargement de l'aliment."));
            }
        }

        /// <summary>
        /// API: Recherche d'aliments par nom
        /// </summary>
        [HttpGet]
        [Route("Api/Search")]
        public async Task<IActionResult> SearchAliments(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term))
                {
                    return Json(ApiResponse<IEnumerable<AlimentDto>>.SuccessResult(new List<AlimentDto>()));
                }

                var aliments = await _alimentService.SearchAlimentsAsync(term);
                return Json(ApiResponse<IEnumerable<AlimentDto>>.SuccessResult(aliments));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche d'aliments avec le terme '{Term}'", term);
                return Json(ApiResponse<IEnumerable<AlimentDto>>.ErrorResult("Erreur lors de la recherche."));
            }
        }

        #endregion

        #region Actions pour la gestion des alertes

        /// <summary>
        /// Active/désactive l'alerte pour un aliment
        /// </summary>
        [HttpPost]
        [Route("Api/ToggleAlerte/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAlerte(int id)
        {
            try
            {
                var result = await _alimentService.ToggleAlerteAsync(id);

                if (result.Success)
                {
                    return Json(CrudResultDto.Success(result.Message, id, result.Data));
                }
                else
                {
                    return Json(CrudResultDto.Error(result.Message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement d'état de l'alerte pour l'aliment {Id}", id);
                return Json(CrudResultDto.Error("Erreur technique lors du changement d'état de l'alerte."));
            }
        }

        /// <summary>
        /// Obtient les aliments avec alerte active
        /// </summary>
        [HttpGet]
        [Route("Api/GetAlertesActives")]
        public async Task<IActionResult> GetAlertesActives()
        {
            try
            {
                var aliments = await _alimentService.GetAlimentsAvecAlerteAsync();
                return Json(ApiResponse<IEnumerable<AlimentDto>>.SuccessResult(aliments));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des aliments avec alerte active");
                return Json(ApiResponse<IEnumerable<AlimentDto>>.ErrorResult("Erreur lors du chargement des alertes."));
            }
        }

        #endregion

        #region Statistiques et reporting

        /// <summary>
        /// Statistiques sur les aliments
        /// </summary>
        [HttpGet]
        [Route("Api/GetStatistiques")]
        public async Task<IActionResult> GetStatistiques()
        {
            try
            {
                var stats = await _alimentService.GetStatistiquesAlimentsAsync();
                return Json(ApiResponse<object>.SuccessResult(stats));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques des aliments");
                return Json(ApiResponse<object>.ErrorResult("Erreur lors du calcul des statistiques."));
            }
        }

        /// <summary>
        /// Export des aliments en CSV
        /// </summary>
        [HttpGet]
        [Route("Export/Csv")]
        public async Task<IActionResult> ExportCsv()
        {
            try
            {
                var csvData = await _alimentService.ExportAlimentsToCsvAsync();
                var fileName = $"aliments_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                return File(csvData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'export CSV des aliments");
                TempData["Error"] = "Erreur lors de l'export.";
                return RedirectToAction(nameof(Index));
            }
        }

        #endregion

        #region Validation côté serveur

        /// <summary>
        /// Valide si le nom d'aliment est unique
        /// </summary>
        [HttpGet]
        [Route("Api/ValidateNomUnique")]
        public async Task<IActionResult> ValidateNomUnique(string name, int? excludeId = null)
        {
            try
            {
                var isUnique = await _alimentService.IsNomAlimentUniqueAsync(name, excludeId);
                return Json(new { isValid = isUnique });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la validation du nom d'aliment '{Name}'", name);
                return Json(new { isValid = false, error = "Erreur lors de la validation." });
            }
        }

        #endregion
    }

    #region ViewModels spécifiques pour AlimentController

    public class CreateAlimentViewModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [MaxLength(25, ErrorMessage = "Le nom ne peut dépasser 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix de référence est obligatoire")]
        [Range(0.01, 999999.99, ErrorMessage = "Le prix doit être compris entre 0.01 et 999,999.99")]
        public decimal PrixReference { get; set; }

        [Required(ErrorMessage = "La quantité minimale est obligatoire")]
        [Range(0, int.MaxValue, ErrorMessage = "La quantité minimale doit être positive")]
        public int QuantiteMinimale { get; set; }

        public int AlerteActive { get; set; } = 0;

        [MaxLength(255, ErrorMessage = "La description ne peut dépasser 255 caractères")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner un type d'aliment")]
        public int IdTypeAliment { get; set; }

        // Pour les listes déroulantes
        public IEnumerable<TypeAlimentDto> TypesAliments { get; set; } = new List<TypeAlimentDto>();
    }

    public class EditAlimentViewModel : CreateAlimentViewModel
    {
        public int Id { get; set; }
    }

    #endregion
}