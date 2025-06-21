using AlimentsUsinages.Web.Models.DTOs;
using AlimentsUsinages.Web.Repositories;

namespace AlimentsUsinages.Web.Services
{
    public class AlimentService : IAlimentService
    {
        private readonly IAlimentRepository _alimentRepository;

        public AlimentService(IAlimentRepository alimentRepository)
        {
            _alimentRepository = alimentRepository;
        }

        public async Task<IEnumerable<AlimentDto>> GetAllAlimentsAsync()
        {
            return await _alimentRepository.GetAllAlimentsAsync();
        }

        public async Task<IEnumerable<AlimentDto>> GetAlimentsByOrigineFormuleAsync(int idOrigineFormule)
        {
            return await _alimentRepository.GetAlimentsByOrigineFormuleAsync(idOrigineFormule);
        }

        public async Task<IEnumerable<OrigineFormuleDto>> GetAllOrigineFormulesAsync()
        {
            return await _alimentRepository.GetAllOrigineFormulesAsync();
        }

        public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
        {
            return await _alimentRepository.GetAllIngredientsAsync();
        }

        public async Task<AlimentDto?> GetAlimentByIdAsync(int id)
        {
            return await _alimentRepository.GetAlimentByIdAsync(id);
        }
    }
}
