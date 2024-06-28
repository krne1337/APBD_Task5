using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using AnimalsDbApi.Models;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;

    public AnimalsController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAnimals([FromQuery] string orderBy = "name")
    {
        var animals = await _animalRepository.GetAnimals(orderBy);
        return Ok(animals);
    }

    [HttpGet("{idAnimal}")]
    public async Task<IActionResult> GetAnimal(int idAnimal)
    {
        var animal = await _animalRepository.GetAnimalById(idAnimal);
        if (animal == null)
            return NotFound();
        return Ok(animal);
    }

    [HttpPost]
    public async Task<IActionResult> AddAnimal([FromBody] Animal animal)
    {
        await _animalRepository.AddAnimal(animal);
        return CreatedAtAction(nameof(GetAnimal), new { idAnimal = animal.IdAnimal }, animal);
    }

    [HttpPut("{idAnimal}")]
    public async Task<IActionResult> UpdateAnimal(int idAnimal, [FromBody] Animal animal)
    {
        var existingAnimal = await _animalRepository.GetAnimalById(idAnimal);
        if (existingAnimal == null)
            return NotFound();

        await _animalRepository.UpdateAnimal(idAnimal, animal);
        return NoContent();
    }

    [HttpDelete("{idAnimal}")]
    public async Task<IActionResult> DeleteAnimal(int idAnimal)
    {
        var existingAnimal = await _animalRepository.GetAnimalById(idAnimal);
        if (existingAnimal == null)
            return NotFound();

        await _animalRepository.DeleteAnimal(idAnimal);
        return NoContent();
    }
}
