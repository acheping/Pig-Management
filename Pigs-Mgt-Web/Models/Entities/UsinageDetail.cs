using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
 
namespace AlimentsUsinages.Web.Models.Entities 
{ 
    [Table("Usinages_Details")] 
    public class UsinageDetail 
    { 
        public int Id { get; set; } 
        [Required] 
        [Column(TypeName = "decimal(10,2)")] 
        public decimal Quantite { get; set; } 
        [Required] 
        public int IdIngredient { get; set; } 
        [Required] 
        public int IdUsinage { get; set; } 
        // Navigation Properties 
        public virtual Ingredient Ingredient { get; set; } = null!; 
        public virtual Usinage Usinage { get; set; } = null!; 
    } 
} 
