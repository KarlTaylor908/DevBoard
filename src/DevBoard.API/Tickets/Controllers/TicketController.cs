using DevBoard.API.Tickets.Requests;
using DevBoard.API.Tickets.Responses;
using DevBoard.Application.Tickets.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.Tickets.Controllers
{
    [ApiController]
    [Route("api/ticket")]
    public class TicketController : Controller
    {
		private readonly ITicketService _ticketService;

		public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket(CreateTicketRequest request)
        {
			try
			{
				var ticket = await _ticketService.CreateTicketAsync(request.Name);

                if (ticket == null)
                {
                    return BadRequest("Create ticket failed.");
                }

				return Json (new TicketResponse()
                {
                    Name = ticket.Name,
                });
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}
        }
    }
}
