//Namespace and Using Statements
using Microsoft.AspNetCore.Mvc;
using ZooAPI.Model;
using ZooAPI.model;
using ZooAPI.Service;

namespace ZooAPI.Controller
//Controller attributes
{
    [ApiController]
    [Route("api/[controller]")] //  Set the base path for this controller: api/animalkeeper
    
    //Dependency Injection
    public class AnimalKeeperController : ControllerBase // Class Animal keeper inherits from ControllerBase
    {
        private readonly AnimalKeeperService _service;
        
        // Constructor: Service injection
        public AnimalKeeperController(AnimalKeeperService service)
        {
            _service = service;
        }

        // Endpoint: GET animals by animal keeper ID
        [HttpGet("{keeperId}/animals")]
        public async Task<ActionResult<List<Animal>>> GetAnimalByKeeperIdAsync(int keeperId)
        {
            try
            {
                return await _service.GetAnimalByKeeperIdAsync(keeperId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Endpoint: PUT -> Update animal
        [HttpPut("animals/{id}/{column}")]
        public async Task<IActionResult> UpdateAnimal(int id, Animal column)
        {
            try
            {
                await _service.UpdateAnimalAsync(id, column);
                return Ok(); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}