using DevBoard.API.Boards.Responses;
using DevBoard.API.Tickets.Requests;
using DevBoard.Application.Boards.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevBoard.API.Boards.Controllers
{
    [ApiController]
    [Route("api/board")]
    public class BoardController : Controller
    { 
        private readonly IBoardService _boardService;
    
        public BoardController(IBoardService boardService) 
        {
        _boardService = boardService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBoard(CreateTicketRequest request)
        {
            try
            {
                var board = await _boardService.CreateBoardAsync(request.Name);

                if (board == null)
                {
                    return BadRequest("Create Board Failed");
                }

                return Json(new BoardResponse()
                {
                    Name = board.Name,
                });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

    }
}
