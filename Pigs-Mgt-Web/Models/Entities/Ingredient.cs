using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
 
namespace AlimentsUsinages.Web.Models.Entities 
{ 
    public class Ingredient 
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
        // Navigation Properties 
        public virtual ICollection<UsinageDetail> UsinageDetails { get; set; } = new List<UsinageDetail>(); 
    } 
} 
