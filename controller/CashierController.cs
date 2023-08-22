//Namespace and Using Statements
using Microsoft.AspNetCore.Mvc;
using ZooAPI.Model;
using ZooAPI.Service;

namespace ZooAPI.Controller
//Controller attributes
{
    [ApiController]
    [Route("api/[controller]")] // Set the base path for this controller: api/cashier
    
    //Dependency Injection
    public class CashierController : ControllerBase
    {
        
        //_service is used to interact with business logic
        private readonly CashierService _service;

        // CashierService injection
        public CashierController(CashierService service)
        {
            _service = service;
        }

        // Ticket purchase Endpoint: api/cashier/buy
        [HttpPost("buy")]
        public async Task<ActionResult<Ticket>> InsertTicket(Ticket ticket) //Receive a Ticket object
        {
            try
            {
                await _service.InsertTicketAsync(ticket);
                return CreatedAtAction(nameof(InsertTicket), new { id = ticket.Id }, ticket); //In case of successful insertion->Status "201 Created" and inserted Ticket.
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // 500 - Internal Server Error status
            }
        }

        // GET All tickets Endpoint: api/cashier/gettickets
        [HttpGet("tickets")]
        public async Task<ActionResult<List<Ticket>>> GetAllTickets()
        {
            try
            {
                return await _service.GetAllSoldTicketsAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET tickets by date Endpoint: api/cashier/tickets/date/{date}
        [HttpGet("tickets/date/{date}")]
        public async Task<ActionResult<List<Ticket>>> GetTicketsByDate(DateTime date)
        {
            try
            {
                var (tickets, total) = await _service.GetTicketsByDate(date);
                return Ok(new { Tickets = tickets, Total = total });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    } 
}