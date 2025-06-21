using System.ComponentModel.DataAnnotations;
using AlimentsUsinages.Web.Models.DTOs;

namespace AlimentsUsinages.Web.Models.ViewModels
{
    public class CreateUsinageViewModel
    {
        [Required(ErrorMessage = "La date est obligatoire")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;
Commande ECHO activ‚e.
        [Required(ErrorMessage = "La quantit‚ voulue est obligatoire")]
        [Range(0.01, 9999999.99, ErrorMessage = "La quantit‚ doit ˆtre comprise entre 0.01 et 9,999,999.99")]
        [Display(Name = "Quantit‚ Voulue")]
        public decimal QuantiteVoulue { get; set; } = 0;
Commande ECHO activ‚e.
        [Range(0, 9999999.99, ErrorMessage = "La quantit‚ r‚elle doit ˆtre positive")]
        [Display(Name = "Quantit‚ R‚elle")]
        public decimal? QuantiteReelle { get; set; }
Commande ECHO activ‚e.
        [MaxLength(2000, ErrorMessage = "Le commentaire ne peut d‚passer 2000 caractŠres")]
        [Display(Name = "Commentaire")]
        public string? Commentaire { get; set; }
Commande ECHO activ‚e.
        [Required(ErrorMessage = "Veuillez s‚lectionner un aliment")]
        [Display(Name = "Aliment")]
        public int IdAliment { get; set; }
Commande ECHO activ‚e.
        [Required(ErrorMessage = "Veuillez s‚lectionner une origine de formule")]
        [Display(Name = "Origine Formule")]
        public int IdOrigineFormule { get; set; }
Commande ECHO activ‚e.
        // Listes pour les ComboBox
        public IEnumerable<OrigineFormuleDto> OrigineFormules { get; set; } = new List<OrigineFormuleDto>();
        public IEnumerable<AlimentDto> Aliments { get; set; } = new List<AlimentDto>();
    }

    public class EditUsinageViewModel : CreateUsinageViewModel
    {
        public int Id { get; set; }
Commande ECHO activ‚e.
        [Display(Name = "Date de cr‚ation")]
        public DateTime? DateCreation { get; set; }
    }
}
