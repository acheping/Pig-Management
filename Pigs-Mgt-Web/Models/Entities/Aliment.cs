using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
 
namespace AlimentsUsinages.Web.Models.Entities 
{ 
    public class Aliment 
    { 
        public int Id { get; set; } 
        [Required] 
        [MaxLength(25)] 
        public string Name { get; set; } = string.Empty; 
        [Required] 
        [Column(TypeName = "decimal(10,2)")] 
        public decimal PrixReference { get; set; } 
        [Required] 
        public int QuantiteMinimale { get; set; } 
        [Required] 
        public int AlerteActive { get; set; } = 0; 
        [MaxLength(255)] 
        public string? Description { get; set; } 
        [Required] 
        public int IdTypeAliment { get; set; } 
        // Navigation Properties 
        public virtual TypeAliment TypeAliment { get; set; } = null!; 
        public virtual ICollection<Usinage> Usinages { get; set; } = new List<Usinage>(); 
    } 
} 
