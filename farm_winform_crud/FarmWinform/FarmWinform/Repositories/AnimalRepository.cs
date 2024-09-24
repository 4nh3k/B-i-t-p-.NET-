using FarmWinform.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FarmWinform.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        public async Task<List<AnimalDTO>> GetAnimalsAsync()
        {
            using (FarmDbEntities db = new FarmDbEntities())
            {
                return await (from a in db.Animals
                              join at in db.AnimalTypes
                              on a.AnimalTypeId equals at.AnimalTypeId
                              select new AnimalDTO
                              {
                                  AnimalId = a.AnimalId,
                                  AnimalTypeId = a.AnimalTypeId ?? 1,
                                  AnimalTypeName = at.AnimalName,
                                  MilkProduced = a.MilkProduced,
                                  OffspringCount = a.OffspringCount
                              }).ToListAsync();
            }
        }

        public async Task<List<AnimalTypeDTO>> GetAnimalTypesAsync()
        {
            using (FarmDbEntities db = new FarmDbEntities())
            {
                return await db.AnimalTypes.Select(at => new AnimalTypeDTO
                {
                    AnimalTypeId = at.AnimalTypeId,
                    AnimalName = at.AnimalName,
                    Sound = at.Sound
                }).ToListAsync();
            }
        }

        public async Task SaveAnimalAsync(AnimalDTO animalDTO)
        {
            using (FarmDbEntities db = new FarmDbEntities())
            {
                Animal animal = new Animal
                {
                    AnimalId = animalDTO.AnimalId,
                    AnimalTypeId = animalDTO.AnimalTypeId,
                    MilkProduced = animalDTO.MilkProduced,
                    OffspringCount = animalDTO.OffspringCount
                };

                if (animal.AnimalId == 0)
{
                    db.Animals.Add(animal);
}                else
{
                    db.Entry(animal).State = EntityState.Modified;
}
                await db.SaveChangesAsync();
            }
        }

        public async Task DeleteAnimalAsync(int animalId)
        {
            using (FarmDbEntities db = new FarmDbEntities())
            {
                var animal = await db.Animals.FirstOrDefaultAsync(a => a.AnimalId == animalId);
                if (animal != null)
                {
                    db.Animals.Remove(animal);
                    await db.SaveChangesAsync();
                }
            }
        }

    }
}