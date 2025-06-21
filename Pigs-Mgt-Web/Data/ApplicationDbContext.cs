using Microsoft.EntityFrameworkCore; 
using AlimentsUsinages.Web.Models.Entities; 
 
namespace AlimentsUsinages.Web.Data 
{ 
    public class ApplicationDbContext : DbContext 
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } 
 
        public DbSet<Usinage> Usinages { get; set; } 
        public DbSet<UsinageDetail> UsinageDetails { get; set; } 
        public DbSet<Aliment> Aliments { get; set; } 
        public DbSet<Ingredient> Ingredients { get; set; } 
        public DbSet<OrigineFormule> OrigineFormules { get; set; } 
        public DbSet<TypeAliment> TypesAliments { get; set; } 
 
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { 
            // Configuration des contraintes et relations 
            // TODO: Ajouter les configurations d‚taill‚es 
            base.OnModelCreating(modelBuilder); 
        } 
    } 
} 
