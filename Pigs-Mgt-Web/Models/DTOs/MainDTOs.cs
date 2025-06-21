namespace AlimentsUsinages.Web.Models.DTOs
{
    public class AlimentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PrixReference { get; set; }
        public string TypeAlimentName { get; set; } = string.Empty;
        public int AlerteActive { get; set; }
        public int QuantiteMinimale { get; set; }
        public string? Description { get; set; }
Commande ECHO activ‚e.
        // Propri‚t‚s pour l'interface
        public string DisplayText => $"{Name} ^({TypeAlimentName}^)";
        public string AlerteBadge => AlerteActive == 1 ? "??" : "";
        public string PrixFormatted => PrixReference.ToString("C");
    }

    public class OrigineFormuleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
Commande ECHO activ‚e.
        public string DisplayText => !string.IsNullOrEmpty(Description) 
            ? $"{Name} - {Description}" 
            : Name;
    }

    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PrixReference { get; set; }
        public int AlerteActive { get; set; }
        public int QuantiteMinimale { get; set; }
        public string? Description { get; set; }
Commande ECHO activ‚e.
        public string DisplayText => Name;
        public string PrixFormatted => PrixReference.ToString("C");
        public string AlerteBadge => AlerteActive == 1 ? "??" : "";
    }

    public class TypeAlimentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
