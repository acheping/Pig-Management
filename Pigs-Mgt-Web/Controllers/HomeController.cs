using Microsoft.AspNetCore.Mvc;
using AlimentsUsinages.Web.Services;
using AlimentsUsinages.Web.Models.ViewModels;
using AlimentsUsinages.Web.Models.DTOs;
using System.Diagnostics;

namespace AlimentsUsinages.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsinageService _usinageService;
        private readonly IAlimentService _alimentService;
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IUsinageService usinageService,
            IAlimentService alimentService,
            IIngredientService ingredientService,
            ILogger<HomeController> logger)
        {
            _usinageService = usinageService;
            _alimentService = alimentService;
            _ingredientService = ingredientService;
            _logger = logger;
        }

        #region Pages principales

        /// <summary>
        /// Page d'accueil avec tableau de bord
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = new DashboardViewModel
                {
                    // Statistiques générales
                    StatistiquesGenerales = await GetStatistiquesGeneralesAsync(),

                    // Derniers usinages
                    DerniersUsinages = await _usinageService.GetDerniersUsinagesAsync(5),

                    // Alertes actives
                    AlertesAliments = await _alimentService.GetAlimentsAvecAlerteAsync(),
                    AlertesIngredients = await _ingredientService.GetIngredientsAvecAlerteAsync(),

                    // Données pour graphiques
                    GraphiqueUsinages = await GetDonneesGraphiqueUsinagesAsync(),
                    GraphiqueAliments = await GetDonneesGraphiqueAlimentsAsync(),

                    // Activité récente
                    ActiviteRecente = await GetActiviteRecenteAsync()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du tableau de bord");
                TempData["Error"] = "Erreur lors du chargement du tableau de bord.";
                return View(new DashboardViewModel());
            }
        }

        /// <summary>
        /// Page À propos
        /// </summary>
        public IActionResult About()
        {
            ViewData["Message"] = "Application de gestion des usinages d'aliments.";

            var model = new AboutViewModel
            {
                ApplicationName = "Aliments Usinages Management",
                Version = GetApplicationVersion(),
                Description = "Système de gestion des usinages d'aliments avec suivi des ingrédients et des formules.",
                Technologies = new List<string>
                {
                    "ASP.NET Core 8.0",
                    "Entity Framework Core",
                    "Bootstrap 5",
                    "jQuery",
                    "Chart.js",
                    "Azure SQL Database"
                },
                Fonctionnalites = new List<string>
                {
                    "Gestion des usinages par date",
                    "Suivi des quantités voulues vs réelles",
                    "Gestion des ingrédients et formules",
                    "Tableaux de bord avec graphiques",
                    "Système d'alertes automatiques",
                    "Export de données",
                    "Interface responsive"
                }
            };

            return View(model);
        }

        /// <summary>
        /// Page de contact/support
        /// </summary>
        public IActionResult Contact()
        {
            ViewData["Message"] = "Contactez notre équipe de support.";
            return View();
        }

        /// <summary>
        /// Page de confidentialité
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        #endregion

        #region API pour le tableau de bord

        /// <summary>
        /// API: Statistiques en temps réel pour le dashboard
        /// </summary>
        [HttpGet]
        [Route("Home/Api/GetStatistiques")]
        public async Task<IActionResult> GetStatistiques()
        {
            try
            {
                var stats = await GetStatistiquesGeneralesAsync();
                return Json(ApiResponse<StatistiquesGeneralesViewModel>.SuccessResult(stats));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul des statistiques générales");
                return Json(ApiResponse<StatistiquesGeneralesViewModel>.ErrorResult("Erreur lors du calcul des statistiques."));
            }
        }

        /// <summary>
        /// API: Données pour graphique des usinages par mois
        /// </summary>
        [HttpGet]
        [Route("Home/Api/GetGraphiqueUsinages")]
        public async Task<IActionResult> GetGraphiqueUsinages(int mois = 12)
        {
            try
            {
                var donnees = await GetDonneesGraphiqueUsinagesAsync(mois);
                return Json(ApiResponse<GraphiqueUsinagesViewModel>.SuccessResult(donnees));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la génération du graphique des usinages");
                return Json(ApiResponse<GraphiqueUsinagesViewModel>.ErrorResult("Erreur lors de la génération du graphique."));
            }
        }

        /// <summary>
        /// API: Données pour graphique répartition par aliment
        /// </summary>
        [HttpGet]
        [Route("Home/Api/GetGraphiqueAliments")]
        public async Task<IActionResult> GetGraphiqueAliments()
        {
            try
            {
                var donnees = await GetDonneesGraphiqueAlimentsAsync();
                return Json(ApiResponse<GraphiqueAlimentsViewModel>.SuccessResult(donnees));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la génération du graphique des aliments");
                return Json(ApiResponse<GraphiqueAlimentsViewModel>.ErrorResult("Erreur lors de la génération du graphique."));
            }
        }

        /// <summary>
        /// API: Alertes en temps réel
        /// </summary>
        [HttpGet]
        [Route("Home/Api/GetAlertes")]
        public async Task<IActionResult> GetAlertes()
        {
            try
            {
                var alertes = new
                {
                    Aliments = await _alimentService.GetAlimentsAvecAlerteAsync(),
                    Ingredients = await _ingredientService.GetIngredientsAvecAlerteAsync(),
                    NombreTotal = 0 // Sera calculé
                };

                var nombreTotal = alertes.Aliments.Count() + alertes.Ingredients.Count();
                var result = new { alertes.Aliments, alertes.Ingredients, NombreTotal = nombreTotal };

                return Json(ApiResponse<object>.SuccessResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des alertes");
                return Json(ApiResponse<object>.ErrorResult("Erreur lors du chargement des alertes."));
            }
        }

        /// <summary>
        /// API: Recherche globale dans l'application
        /// </summary>
        [HttpGet]
        [Route("Home/Api/Search")]
        public async Task<IActionResult> Search(string term)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                {
                    return Json(ApiResponse<object>.SuccessResult(new { }));
                }

                var resultats = new SearchResultViewModel
                {
                    Usinages = await _usinageService.SearchUsinagesAsync(term),
                    Aliments = await _alimentService.SearchAlimentsAsync(term),
                    Ingredients = await _ingredientService.SearchIngredientsAsync(term)
                };

                return Json(ApiResponse<SearchResultViewModel>.SuccessResult(resultats));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche globale avec le terme '{Term}'", term);
                return Json(ApiResponse<SearchResultViewModel>.ErrorResult("Erreur lors de la recherche."));
            }
        }

        #endregion

        #region Gestion des erreurs

        /// <summary>
        /// Page d'erreur
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ShowRequestId = !string.IsNullOrEmpty(Activity.Current?.Id ?? HttpContext.TraceIdentifier)
            };

            return View(model);
        }

        /// <summary>
        /// Page 404 - Non trouvé
        /// </summary>
        [Route("Home/NotFound")]
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        /// <summary>
        /// Page 403 - Accès refusé
        /// </summary>
        [Route("Home/AccessDenied")]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }

        #endregion

        #region Actions utilitaires

        /// <summary>
        /// Test de la connexion à la base de données
        /// </summary>
        [HttpGet]
        [Route("Home/Api/TestConnection")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                var isConnected = await _usinageService.TestDatabaseConnectionAsync();

                if (isConnected)
                {
                    return Json(ApiResponse<object>.SuccessResult(new { Status = "Connected", Message = "Connexion à la base de données réussie" }));
                }
                else
                {
                    return Json(ApiResponse<object>.ErrorResult("Impossible de se connecter à la base de données"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du test de connexion à la base de données");
                return Json(ApiResponse<object>.ErrorResult("Erreur lors du test de connexion"));
            }
        }

        /// <summary>
        /// Informations système
        /// </summary>
        [HttpGet]
        [Route("Home/Api/SystemInfo")]
        public IActionResult GetSystemInfo()
        {
            try
            {
                var info = new
                {
                    ApplicationName = "Aliments Usinages",
                    Version = GetApplicationVersion(),
                    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                    ServerTime = DateTime.Now,
                    Uptime = GetApplicationUptime(),
                    Culture = System.Globalization.CultureInfo.CurrentCulture.Name
                };

                return Json(ApiResponse<object>.SuccessResult(info));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des informations système");
                return Json(ApiResponse<object>.ErrorResult("Erreur lors de la récupération des informations système"));
            }
        }

        #endregion

        #region Méthodes privées pour calculs

        /// <summary>
        /// Calcule les statistiques générales
        /// </summary>
        private async Task<StatistiquesGeneralesViewModel> GetStatistiquesGeneralesAsync()
        {
            var stats = new StatistiquesGeneralesViewModel();

            // Statistiques des usinages
            var usinages = await _usinageService.GetAllUsinagesAsync();
            stats.NombreUsinages = usinages.Count();
            stats.UsinagesAujourdhui = usinages.Count(u => u.Date.Date == DateTime.Today);
            stats.UsinagesCetteSemaine = usinages.Count(u => u.Date >= DateTime.Today.AddDays(-7));

            // Quantités
            stats.QuantiteTotaleVoulue = usinages.Sum(u => u.QuantiteVoulue);
            stats.QuantiteTotaleReelle = usinages.Sum(u => u.QuantiteReelle ?? 0);
            stats.EcartMoyen = stats.QuantiteTotaleVoulue > 0
                ? ((stats.QuantiteTotaleReelle - stats.QuantiteTotaleVoulue) / stats.QuantiteTotaleVoulue) * 100
                : 0;

            // Aliments et ingrédients
            var aliments = await _alimentService.GetAllAlimentsAsync();
            var ingredients = await _ingredientService.GetAllIngredientsAsync();

            stats.NombreAliments = aliments.Count();
            stats.NombreIngredients = ingredients.Count();
            stats.NombreAlertesActives = aliments.Count(a => a.AlerteActive == 1) + ingredients.Count(i => i.AlerteActive == 1);

            // Évolution (comparaison avec le mois précédent)
            var moisPrecedent = DateTime.Today.AddMonths(-1);
            var usinagesMoisPrecedent = usinages.Count(u => u.Date.Month == moisPrecedent.Month && u.Date.Year == moisPrecedent.Year);
            var usinagesMoisActuel = usinages.Count(u => u.Date.Month == DateTime.Today.Month && u.Date.Year == DateTime.Today.Year);

            stats.EvolutionUsinages = usinagesMoisPrecedent > 0
                ? ((usinagesMoisActuel - usinagesMoisPrecedent) / (decimal)usinagesMoisPrecedent) * 100
                : 0;

            return stats;
        }

        /// <summary>
        /// Génère les données pour le graphique des usinages
        /// </summary>
        private async Task<GraphiqueUsinagesViewModel> GetDonneesGraphiqueUsinagesAsync(int nombreMois = 12)
        {
            var usinages = await _usinageService.GetAllUsinagesAsync();
            var donnees = new GraphiqueUsinagesViewModel();

            var dateDebut = DateTime.Today.AddMonths(-nombreMois);

            for (int i = 0; i < nombreMois; i++)
            {
                var date = dateDebut.AddMonths(i);
                var usinagesMois = usinages.Where(u => u.Date.Month == date.Month && u.Date.Year == date.Year);

                donnees.Labels.Add(date.ToString("MMM yyyy"));
                donnees.NombreUsinages.Add(usinagesMois.Count());
                donnees.QuantiteVoulue.Add(usinagesMois.Sum(u => u.QuantiteVoulue));
                donnees.QuantiteReelle.Add(usinagesMois.Sum(u => u.QuantiteReelle ?? 0));
            }

            return donnees;
        }

        /// <summary>
        /// Génère les données pour le graphique des aliments
        /// </summary>
        private async Task<GraphiqueAlimentsViewModel> GetDonneesGraphiqueAlimentsAsync()
        {
            var usinages = await _usinageService.GetAllUsinagesAsync();
            var donnees = new GraphiqueAlimentsViewModel();

            var groupesAliments = usinages
                .GroupBy(u => u.AlimentName)
                .OrderByDescending(g => g.Sum(u => u.QuantiteVoulue))
                .Take(10) // Top 10 des aliments
                .ToList();

            foreach (var groupe in groupesAliments)
            {
                donnees.Labels.Add(groupe.Key);
                donnees.Quantites.Add(groupe.Sum(u => u.QuantiteVoulue));
                donnees.NombreUsinages.Add(groupe.Count());
            }

            return donnees;
        }

        /// <summary>
        /// Récupère l'activité récente
        /// </summary>
        private async Task<List<ActiviteRecenteViewModel>> GetActiviteRecenteAsync()
        {
            var activites = new List<ActiviteRecenteViewModel>();

            // Derniers usinages créés
            var derniersUsinages = await _usinageService.GetDerniersUsinagesAsync(5);
            foreach (var usinage in derniersUsinages)
            {
                activites.Add(new ActiviteRecenteViewModel
                {
                    Type = "Usinage",
                    Description = $"Usinage de {usinage.AlimentName} - {usinage.QuantiteVoulue:N2}",
                    Date = usinage.Date,
                    Icone = "??",
                    Url = $"/Usinage/Edit/{usinage.Id}"
                });
            }

            // Alertes récentes
            var alertesAliments = await _alimentService.GetAlimentsAvecAlerteAsync();
            foreach (var aliment in alertesAliments.Take(3))
            {
                activites.Add(new ActiviteRecenteViewModel
                {
                    Type = "Alerte",
                    Description = $"Alerte active pour {aliment.Name}",
                    Date = DateTime.Now, // Vous pourriez avoir une date d'activation de l'alerte
                    Icone = "??",
                    Url = $"/Aliment/Edit/{aliment.Id}"
                });
            }

            return activites.OrderByDescending(a => a.Date).Take(10).ToList();
        }

        /// <summary>
        /// Récupère la version de l'application
        /// </summary>
        private string GetApplicationVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            return version?.ToString() ?? "1.0.0.0";
        }

        /// <summary>
        /// Calcule l'uptime de l'application
        /// </summary>
        private TimeSpan GetApplicationUptime()
        {
            var process = Process.GetCurrentProcess();
            return DateTime.Now - process.StartTime;
        }

        #endregion
    }

    #region ViewModels pour HomeController

    public class DashboardViewModel
    {
        public StatistiquesGeneralesViewModel StatistiquesGenerales { get; set; } = new();
        public IEnumerable<UsinageViewModel> DerniersUsinages { get; set; } = new List<UsinageViewModel>();
        public IEnumerable<AlimentDto> AlertesAliments { get; set; } = new List<AlimentDto>();
        public IEnumerable<IngredientDto> AlertesIngredients { get; set; } = new List<IngredientDto>();
        public GraphiqueUsinagesViewModel GraphiqueUsinages { get; set; } = new();
        public GraphiqueAlimentsViewModel GraphiqueAliments { get; set; } = new();
        public List<ActiviteRecenteViewModel> ActiviteRecente { get; set; } = new();
    }

    public class StatistiquesGeneralesViewModel
    {
        public int NombreUsinages { get; set; }
        public int UsinagesAujourdhui { get; set; }
        public int UsinagesCetteSemaine { get; set; }
        public decimal QuantiteTotaleVoulue { get; set; }
        public decimal QuantiteTotaleReelle { get; set; }
        public decimal EcartMoyen { get; set; }
        public int NombreAliments { get; set; }
        public int NombreIngredients { get; set; }
        public int NombreAlertesActives { get; set; }
        public decimal EvolutionUsinages { get; set; }
    }

    public class GraphiqueUsinagesViewModel
    {
        public List<string> Labels { get; set; } = new();
        public List<int> NombreUsinages { get; set; } = new();
        public List<decimal> QuantiteVoulue { get; set; } = new();
        public List<decimal> QuantiteReelle { get; set; } = new();
    }

    public class GraphiqueAlimentsViewModel
    {
        public List<string> Labels { get; set; } = new();
        public List<decimal> Quantites { get; set; } = new();
        public List<int> NombreUsinages { get; set; } = new();
    }

    public class ActiviteRecenteViewModel
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Icone { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class SearchResultViewModel
    {
        public IEnumerable<UsinageViewModel> Usinages { get; set; } = new List<UsinageViewModel>();
        public IEnumerable<AlimentDto> Aliments { get; set; } = new List<AlimentDto>();
        public IEnumerable<IngredientDto> Ingredients { get; set; } = new List<IngredientDto>();
    }

    public class AboutViewModel
    {
        public string ApplicationName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Technologies { get; set; } = new();
        public List<string> Fonctionnalites { get; set; } = new();
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    #endregion
}