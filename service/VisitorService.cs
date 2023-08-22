using ZooAPI.Controller;
using ZooAPI.Model;
using ZooAPI.model;

namespace ZooAPI.Service
{
    public class VisitorService
    {
        private readonly DBConnection _dbConnection;

        // Constructor: DB Connection Injection
        public VisitorService(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        //Retrieve all animals
        public async Task<List<Animal>> GetAllAnimals()
        {
            var result = new List<Animal>();
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            
            // SQL query: all animals
            command.CommandText = "SELECT * FROM Zoo.animals";
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

        // Retrieve animal by species
        public async Task<Animal?> GetAnimalBySpecies(string species)
        {
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            
            // SQL query: animal by species
            command.CommandText = "SELECT * FROM Zoo.animals WHERE species = @species";
            command.Parameters.AddWithValue("@species", species);
            await using var reader = await command.ExecuteReaderAsync();
            Animal? animal = null;
            if (await reader.ReadAsync())
            {
                animal = new Animal(
                    reader.GetInt32("id"),
                    reader.GetString("species"),
                    reader.GetString("food"),
                    reader.GetInt32("enclosure_id")
                );
            }

            return animal;
        }
    }
}