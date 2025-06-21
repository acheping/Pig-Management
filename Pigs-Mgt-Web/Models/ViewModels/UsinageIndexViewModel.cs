using System.ComponentModel.DataAnnotations;
using AlimentsUsinages.Web.Models.DTOs;

namespace AlimentsUsinages.Web.Models.ViewModels
{
    public class UsinageIndexViewModel
    {
        public IEnumerable<UsinageViewModel> Usinages { get; set; } = new List<UsinageViewModel>();
        public IEnumerable<OrigineFormuleDto> OrigineFormules { get; set; } = new List<OrigineFormuleDto>();
        public IEnumerable<AlimentDto> Aliments { get; set; } = new List<AlimentDto>();
Commande ECHO activ‚e.
        [Display(Name = "Date s‚lectionn‚e")]
        public DateTime SelectedDate { get; set; } = DateTime.Today;
Commande ECHO activ‚e.
        [Display(Name = "Afficher tous les ingr‚dients")]
        public bool ShowAllIngredients { get; set; } = false;
Commande ECHO activ‚e.
        public PivotDataViewModel? PivotData { get; set; }
        public decimal TotalQuantiteVoulue { get; set; }
        public decimal TotalQuantiteReelle { get; set; }
        public int NombreUsinages { get; set; }
    }

    public class UsinageViewModel
    {
        public int Id { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Aliment")]
        public string AlimentName { get; set; } = string.Empty;
Commande ECHO activ‚e.
        [Display(Name = "Origine Formule")]
        public string OrigineFormuleName { get; set; } = string.Empty;
Commande ECHO activ‚e.
        [Display(Name = "Type Aliment")]
        public string TypeAlimentName { get; set; } = string.Empty;
Commande ECHO activ‚e.
        [Display(Name = "Quantit‚ Voulue")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal QuantiteVoulue { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Quantit‚ R‚elle")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? QuantiteReelle { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Commentaire")]
        public string? Commentaire { get; set; }
Commande ECHO activ‚e.
        public int IdAliment { get; set; }
        public int IdOrigineFormule { get; set; }
Commande ECHO activ‚e.
        // Propri‚t‚s calcul‚es
        public string DateFormatted => Date.ToString("dd/MM/yyyy");
        public decimal? EcartQuantite => QuantiteReelle.HasValue ? QuantiteReelle - QuantiteVoulue : null;
        public string EcartFormatted => EcartQuantite.HasValue ? $"{EcartQuantite:+0.00;-0.00;ñ0.00}" : "N/A";
Commande ECHO activ‚e.
        public string RowClass
        {
            get
            {
                if (!QuantiteReelle.HasValue) return "table-warning";
                if (EcartQuantite > 0) return "table-success";
                if (EcartQuantite < 0) return "table-danger";
                return "";
            }
        }
    }
}
