using Microsoft.Extensions.Options;
using ZooAPI.Model;
using ZooAPI.Controller;

namespace ZooAPI.Service
{
    // Service for cashier functions
    public class CashierService
    {
        private readonly DBConnection _dbConnection; // Database connection
        private readonly IOptions<TicketPrices> _ticketPrices; // Ticket prices

        // Constructor: DB & ticket prices 
        public CashierService(DBConnection dbConnection, IOptions<TicketPrices> ticketPrices)
        {
            _dbConnection = dbConnection;
            _ticketPrices = ticketPrices;
        }

        // Ticket purchase
        public async Task BuyTicket(TicketType type)
        {
            // Define ticket price
            var ticketPrice = type switch
            {
                TicketType.Children => _ticketPrices.Value.Children,
                TicketType.Adult => _ticketPrices.Value.Adult,
                TicketType.Senior => _ticketPrices.Value.Senior,
                _ => throw new ArgumentException("Invalid ticket type")
            };

            // Create a ticket
            var ticket = new Ticket
            {
                Type = type,
                Price = ticketPrice,
                DateOfSale = DateTime.Now
            };

            // Insert ticket into DB
            await InsertTicketAsync(ticket);
        }

        // Get all tickets that were sold
        public async Task<List<Ticket>> GetAllSoldTicketsAsync()
        {
            var result = new List<Ticket>();
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            
            // SQL: all tickets
            command.CommandText = "SELECT * FROM Zoo.tickets";
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var ticket = new Ticket
                {
                    Id = reader.GetInt32("id"),
                    Type = reader.IsDBNull(reader.GetOrdinal("type"))
                        ? default
                        : (TicketType)Enum.Parse(typeof(TicketType), reader.GetString("type")),
                    Price = reader.GetDecimal("price"),
                    DateOfSale = reader.GetDateTime("dateOfSale")
                };
                result.Add(ticket);
            }

            return result;
        }

        // Insert ticket into DB
        public async Task InsertTicketAsync(Ticket ticket)
        {
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            
            // SQL insert
            command.CommandText =
                "INSERT INTO Zoo.tickets (type, price, dateOfSale) VALUES (@type, @price, @dateOfSale)";
            command.Parameters.AddWithValue("@type", ticket.Type);
            command.Parameters.AddWithValue("@price", ticket.Price);
            command.Parameters.AddWithValue("@dateOfSale", ticket.DateOfSale);
            await command.ExecuteNonQueryAsync();
        }

        /// Get tickets by date and calculate total price
        public async Task<(List<Ticket> Tickets, decimal Total)> GetTicketsByDate(DateTime date)
        {
            var tickets = new List<Ticket>();
            decimal total = 0;
            await using var conn = await _dbConnection.GetConnectionAsync();
            await using var command = conn.CreateCommand();
            
            // SQL query: tickets by date
            command.CommandText = "SELECT * FROM Zoo.tickets WHERE dateOfSale = @date";
            command.Parameters.AddWithValue("@date", date);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var ticket = new Ticket
                {
                    Id = reader.GetInt32("id"),
                    Price = reader.GetDecimal("price"),
                    DateOfSale = reader.GetDateTime("dateOfSale")
                };
                tickets.Add(ticket);
                total += ticket.Price;
            }

            return (tickets, Total: total);
        }
    }
}