using DevBoard.API.Tickets.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.Tickets.Controllers
{
    public class TicketController : Controller
    {
        public async Task<IActionResult> CreateTicket(CreateTicketRequest request)
        {
			try
			{
				var ticket = await _ticketService.CreateTicketAsync(request.Name);
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
        }
    }
}
