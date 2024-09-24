using FarmWinform.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmWinform.Repositories
{
    public interface IAnimalRepository
    {
        Task<List<AnimalDTO>> GetAnimalsAsync();
        Task<List<AnimalTypeDTO>> GetAnimalTypesAsync();
        Task SaveAnimalAsync(AnimalDTO animalDTO);
        Task DeleteAnimalAsync(int animalId);
    }
}
