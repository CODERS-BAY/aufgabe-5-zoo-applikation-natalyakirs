using ZooAPI.Model;
using ZooAPI.Controller;
using ZooAPI.model;

namespace ZooAPI.Service
{
    public class AnimalKeeperService
    {
        private readonly DBConnection _dbConnection;

        // Constructor: Injection of the database connection
        public AnimalKeeperService(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Method for retrieving animals based on the keeper ID
        public async Task<List<Animal>> GetAnimalByKeeperIdAsync(int keeperId)
        {
            var result = new List<Animal>();
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            
            // SQL query: animals based on the keeper ID
            command.CommandText =
                "SELECT t.* FROM Zoo.animals t JOIN Zoo.enclosure g ON t.enclosure_id = g.Id JOIN Zoo.employee m ON g.employee_id = m.id WHERE m.id = @keeperId";
            command.Parameters.AddWithValue("@keeperId", keeperId);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var animal = new Animal(
                    reader.GetInt32("id"),
                    reader.GetString("species"),
                    reader.GetString("food"),
                    reader.GetInt32("enclosure_id")
                    
                );
                result.Add(animal);
            }

            return result;
        }

        // Method to update an animal in the database
        public async Task UpdateAnimalAsync(int id, Animal updatedAnimal)
        {
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            
            // SQL update command for the animal
            command.CommandText =
                "UPDATE Zoo.animals SET species = @species, food = @food, enclosure_id = @enclosureId WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@species", updatedAnimal.Species);
            command.Parameters.AddWithValue("@food", updatedAnimal.Food);
            command.Parameters.AddWithValue("@enclosureId", updatedAnimal.EnclosureId);
            await command.ExecuteNonQueryAsync();
        }
    }
}