using Microsoft.AspNetCore.Mvc;
using ZooAPI.Model;
using ZooAPI.model;
using ZooAPI.Service;

namespace ZooAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")] //Set the base path for this controller
    public class VisitorController : ControllerBase
    {
        private readonly VisitorService _service;

        // Service injection
        public VisitorController(VisitorService service)
        {
            _service = service;
        }

        // GET All Animals Endpoint
        [HttpGet("animals")] // api/visitor/animals
        public async Task<ActionResult<List<Animal>>> GetAllAnimals()
        {
            return await _service.GetAllAnimals();
        }

        // GET animal by species Endpoint
        [HttpGet("animals/{species}")] // api/visitor/animals/{species}
        public async Task<ActionResult<Animal>> GetAnimalBySpecies(string species)
        {
            var animal = await _service.GetAnimalBySpecies(species);
            if (animal == null)
            {
                return NotFound();
            }

            return animal;
        }
    }
}