using System.ComponentModel.DataAnnotations;

namespace AlimentsUsinages.Web.Models.ViewModels
{
    // quivalent de votre dgvAll avec donn‚es pivot
    public class PivotDataViewModel
    {
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
Commande ECHO activ‚e.
        // Donn‚es pivot: [Ingr‚dient][Aliment] = Quantit‚
        public Dictionary<string, Dictionary<string, decimal>> Data { get; set; } = new();
Commande ECHO activ‚e.
        // Totaux par ingr‚dient
        public Dictionary<string, decimal> TotalsByIngredient { get; set; } = new();
Commande ECHO activ‚e.
        // Totaux par aliment
        public Dictionary<string, decimal> TotalsByAliment { get; set; } = new();
Commande ECHO activ‚e.
        // Liste des aliments pour les colonnes
        public List<string> AlimentNames { get; set; } = new();
Commande ECHO activ‚e.
        // Liste des ingr‚dients pour les lignes
        public List<string> IngredientNames { get; set; } = new();
Commande ECHO activ‚e.
        // Grand total
        public decimal GrandTotal => TotalsByIngredient.Values.Sum();
Commande ECHO activ‚e.
        // M‚thodes utilitaires pour l'affichage
        public decimal GetQuantite(string ingredient, string aliment)
        {
            return Data.ContainsKey(ingredient) ^
                ? Data[ingredient][aliment] 
                : 0;
        }
Commande ECHO activ‚e.
        public string GetFormattedQuantite(string ingredient, string aliment)
        {
            var quantite = GetQuantite(ingredient, aliment);
            return quantite == 0 ? "" : quantite.ToString("N2");
        }
Commande ECHO activ‚e.
        // Style conditionnel pour les cellules
        public string GetCellClass(string ingredient, string aliment)
        {
            var quantite = GetQuantite(ingredient, aliment);
            if (quantite == 0) return "text-muted";
            if (quantite < 1) return "text-warning";
            return "text-primary fw-bold";
        }
    }

    // Pour l'affichage en lignes
    public class PivotRowViewModel
    {
        public string IngredientName { get; set; } = string.Empty;
        public Dictionary<string, decimal> AlimentQuantites { get; set; } = new();
        public decimal TotalIngredient { get; set; }
        public bool IsGrandTotal { get; set; }
Commande ECHO activ‚e.
        public string RowClass => IsGrandTotal ? "table-dark fw-bold" : "";
    }
}
