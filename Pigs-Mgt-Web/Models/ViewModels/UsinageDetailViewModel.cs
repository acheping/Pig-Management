using System.ComponentModel.DataAnnotations;

namespace AlimentsUsinages.Web.Models.ViewModels
{
    // quivalent de votre dgvDetail
    public class UsinageDetailViewModel
    {
        public int Id { get; set; }
        public int IdUsinage { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Ingr‚dient")]
        public string IngredientName { get; set; } = string.Empty;
Commande ECHO activ‚e.
        [Display(Name = "Quantit‚")]
        [Range(0, 9999999.99, ErrorMessage = "La quantit‚ doit ˆtre positive")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Quantite { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Quantit‚ Formule")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal QuantiteFormule { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Prix Unitaire")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal PrixUnitaire { get; set; }
Commande ECHO activ‚e.
        public int IdIngredient { get; set; }
        public int AlerteActive { get; set; }
Commande ECHO activ‚e.
        // Propri‚t‚s pour l'‚dition
        public bool IsNew => Id == 0;
        public bool IsModified { get; set; }
        public bool IsDeleted { get; set; }
Commande ECHO activ‚e.
        // Co–t calcul‚
        public decimal CoutTotal => Quantite * PrixUnitaire;
Commande ECHO activ‚e.
        // Style conditionnel
        public string RowClass
        {
            get
            {
                if (IsDeleted) return "table-danger";
                if (IsNew) return "table-success";
                if (IsModified) return "table-warning";
                if (AlerteActive == 1 ^
                return "";
            }
        }
    }

    // Container pour les d‚tails avec totaux (‚quivalent tbTotal)
    public class UsinageDetailContainerViewModel
    {
        public int UsinageId { get; set; }
        public string AlimentName { get; set; } = string.Empty;
        public decimal QuantiteVoulue { get; set; }
        public bool ShowAllIngredients { get; set; }
Commande ECHO activ‚e.
        public IEnumerable<UsinageDetailViewModel> Details { get; set; } = new List<UsinageDetailViewModel>();
Commande ECHO activ‚e.
        // Totaux (‚quivalent de votre tbTotal)
        public decimal TotalQuantite => Details.Where(d => !d.IsDeleted).Sum(d => d.Quantite);
        public decimal TotalCout => Details.Where(d => !d.IsDeleted).Sum(d => d.CoutTotal);
        public decimal PourcentageUtilisation => QuantiteVoulue > 0 ? (TotalQuantite / QuantiteVoulue) * 100 : 0;
Commande ECHO activ‚e.
        // Contr“le qualit‚
        public string ControleStatus
        {
            get
            {
                var ecart = Math.Abs(PourcentageUtilisation - 100);
                if (ecart <= 2) return "Parfait";
                if (ecart <= 5) return "Acceptable";
                return "· v‚rifier";
            }
        }
Commande ECHO activ‚e.
        public string ControleClass
        {
            get
            {
                var ecart = Math.Abs(PourcentageUtilisation - 100);
                if (ecart <= 2) return "text-success";
                if (ecart <= 5) return "text-warning";
                return "text-danger";
            }
        }
    }
}
