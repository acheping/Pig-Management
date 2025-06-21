using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema; 
 
namespace AlimentsUsinages.Web.Models.Entities 
{ 
    public class Usinage 
    { 
        public int Id { get; set; } 
        [Required] 
        public DateTime Date { get; set; } 
        [Required] 
        [Column(TypeName = "decimal(10,2)")] 
        public decimal QuantiteVoulue { get; set; } 
        [Column(TypeName = "decimal(10,2)")] 
        public decimal? QuantiteReelle { get; set; } 
        [MaxLength(2000)] 
        public string? Commentaire { get; set; } 
        [Required] 
        public int IdAliment { get; set; } 
        [Required] 
        public int IdOrigineFormule { get; set; } 
        // Navigation Properties 
        public virtual Aliment Aliment { get; set; } = null!; 
        public virtual OrigineFormule OrigineFormule { get; set; } = null!; 
        public virtual ICollection<UsinageDetail> UsinageDetails { get; set; } = new List<UsinageDetail>(); 
    } 
} 
