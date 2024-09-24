using FarmWinform.Dtos;
using FarmWinform.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmWinform.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private static readonly Random _random = new Random();

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public async Task<List<AnimalDTO>> GetAllAnimalsAfterBreedingAndMilkingAsync()
        {
            var animals = await _animalRepository.GetAnimalsAsync();
            if (animals == null || animals.Count == 0)
                return animals;

            foreach (var animal in animals)
            {
                animal.OffspringGenerated = _random.Next(1, 5);

                switch (animal.AnimalTypeName)
                {
                    case "Bò":
                        animal.MilkProducedInRound = Math.Round(_random.NextDouble() * 20, 2);
                        break;
                    case "Cừu":
                        animal.MilkProducedInRound = Math.Round(_random.NextDouble() * 5, 2); 
                        break;
                    case "Dê":
                        animal.MilkProducedInRound = Math.Round(_random.NextDouble() * 10, 2);
                        break;
                    default:
                        animal.MilkProducedInRound = 0;
                        break;
                }

                animal.OffspringCount += animal.OffspringGenerated;
                animal.MilkProduced += animal.MilkProducedInRound;
            }

            return animals;
        }

        public async Task<string> GetFarmSoundsAsync()
        {
            var animalTypes = await _animalRepository.GetAnimalTypesAsync();
            var sounds = animalTypes.Select(at => at.Sound).ToList();

            return string.Join(", ", sounds);
        }

        public async Task<List<AnimalDTO>> GetAllAnimalsAsync()
        {
            return await _animalRepository.GetAnimalsAsync();
        }

        public async Task<List<AnimalTypeDTO>> GetAllAnimalTypesAsync()
        {
            return await _animalRepository.GetAnimalTypesAsync();
        }

        public async Task SaveAnimalAsync(AnimalDTO animalDTO)
        {
            await _animalRepository.SaveAnimalAsync(animalDTO);
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            await _animalRepository.DeleteAnimalAsync(animalId);
        }
    }
}
