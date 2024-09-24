using FarmWinform.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FarmWinform.Services
{
    public interface IAnimalService
    {
        Task<List<AnimalDTO>> GetAllAnimalsAfterBreedingAndMilkingAsync();
        Task<string> GetFarmSoundsAsync();
        Task<List<AnimalDTO>> GetAllAnimalsAsync();
        Task<List<AnimalTypeDTO>> GetAllAnimalTypesAsync();
        Task SaveAnimalAsync(AnimalDTO animalDTO);
        Task DeleteAnimalAsync(int animalId);
    }
}
