using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
 
namespace AlimentsUsinages.Web.Models.Entities 
{ 
    [Table("Types_Aliments")] 
    public class TypeAliment 
    { 
        public int Id { get; set; } 
        [Required] 
        [MaxLength(25)] 
        public string Name { get; set; } = string.Empty; 
        [MaxLength(255)] 
        public string? Description { get; set; } 
        // Navigation Properties 
        public virtual ICollection<Aliment> Aliments { get; set; } = new List<Aliment>(); 
    } 
} 
