using AnimalsDbApi.Models;

public interface IAnimalRepository
{
    Task<IEnumerable<Animal>> GetAnimals(string orderBy);
    Task<Animal> GetAnimalById(int idAnimal);
    Task AddAnimal(Animal animal);
    Task UpdateAnimal(int idAnimal, Animal animal);
    Task DeleteAnimal(int idAnimal);
}

public class AnimalRepository : IAnimalRepository
{
    private readonly string _connectionString;

    public AnimalRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Animal>> GetAnimals(string orderBy)
    {
        var animals = new List<Animal>();
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT IdAnimal, Name, Description, Category, Area FROM Animals ORDER BY " +
                        (orderBy ?? "Name");

            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        animals.Add(new Animal
                        {
                            IdAnimal = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Category = reader.GetString(3),
                            Area = reader.GetString(4)
                        });
                    }
                }
            }
        }
        return animals;
    }

    public async Task<Animal> GetAnimalById(int idAnimal)
    {
        Animal animal = null;
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "SELECT IdAnimal, Name, Description, Category, Area FROM Animals WHERE IdAnimal = @IdAnimal";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        animal = new Animal
                        {
                            IdAnimal = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Category = reader.GetString(3),
                            Area = reader.GetString(4)
                        };
                    }
                }
            }
        }
        return animal;
    }

    public async Task AddAnimal(Animal animal)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "INSERT INTO Animals (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task UpdateAnimal(int idAnimal, Animal animal)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "UPDATE Animals SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                command.Parameters.AddWithValue("@Name", animal.Name);
                command.Parameters.AddWithValue("@Description", animal.Description);
                command.Parameters.AddWithValue("@Category", animal.Category);
                command.Parameters.AddWithValue("@Area", animal.Area);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task DeleteAnimal(int idAnimal)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var query = "DELETE FROM Animals WHERE IdAnimal = @IdAnimal";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@IdAnimal", idAnimal);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
