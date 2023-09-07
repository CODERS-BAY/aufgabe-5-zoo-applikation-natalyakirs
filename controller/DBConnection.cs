// Using Directives
using System.Data;
using MySqlConnector;


//Namespace and Class declaration
namespace ZooAPI.Controller
{ 
    // Database connection class
    public class DBConnection
    {
        private readonly string _connectionString; // connection string to the database

        // Constructor with configuration
        public DBConnection(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("ZooDb"); // Get connection string from configuration; connection string is retrieved by using the key "ZooDb".
        }

        
        // Constructor with connection string and configuration
        public DBConnection(string connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
        }

        // Asynchronous method to establish secure database connection
        public async Task<MySqlConnection> GetConnectionAsync()
        {
            //Check if connection string is valid
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new Exception($"Connection string is undefined" +$".");
            }
            
            var conn = new MySqlConnection(_connectionString); // Create a new connection object

            await conn.OpenAsync(); // Open connection asynchronously

            //Check if connection state is open
            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("Could not open connection!");
            }

            return conn;  // Return established connection
        }
    }
}