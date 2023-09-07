namespace ZooAPI.Model
{
    // Ticket-Modell
    public class Ticket
    {
        public int Id { get; set; } // Ticket-ID
        public TicketType Type { get; set; } // Ticket types (Children, Adult, Senior)
        public decimal Price { get; set; } // The price  of the ticket
        public DateTime DateOfSale { get; set; } // Date of sale of the ticket
    }

    // Prices of different types of tickets
    public class TicketPrices
    {
        public decimal Children { get; set; } // Price for children
        public decimal Adult { get; set; } // Price for adult
        public decimal Senior { get; set; } // Price for senior
    }

    // Ticket types
    public enum TicketType
    {
        Children, // Children ticket = 1
        Adult, // Adult ticket = 2
        Senior // Senior ticket = 3
    }
}