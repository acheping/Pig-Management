using AlimentsUsinages.Web.Models.DTOs;

namespace AlimentsUsinages.Web.Repositories
{
    public interface IAlimentRepository
    {
        Task<IEnumerable<AlimentDto>> GetAllAlimentsAsync();
        Task<IEnumerable<AlimentDto>> GetAlimentsByOrigineFormuleAsync(int idOrigineFormule);
        Task<IEnumerable<OrigineFormuleDto>> GetAllOrigineFormulesAsync();
        Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync();
        Task<AlimentDto?> GetAlimentByIdAsync(int id);
    }
}
