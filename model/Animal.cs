namespace ZooAPI.model;

public class Animal 
{
    // Constructor
    public Animal(int id, string species, string food, int enclosureId)
    {
        Id = id; // Animal ID
        Species = species; // Animal species 
        Food = food; // Food of the animal
        EnclosureId = enclosureId; // ID of the enclosure the animal is in
    }

    public int Id { get; set; } // animal ID
    public string Species { get; set; } // animal species
    public string Food { get; set; } // animal food
    public int EnclosureId { get; set; } // enclosure ID
}