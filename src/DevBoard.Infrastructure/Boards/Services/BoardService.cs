using DevBoard.Application.Boards.Interfaces;
using DevBoard.Domain.Boards;
using DevBoard.Domain.Boards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Infrastructure.Boards.Services
{
    public class BoardService : IBoardService
    {
        private readonly IBoardRepository _boardRepository;

        public BoardService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        public async Task<BoardEnt> CreateBoardAsync(string name)
        {
            var board = new BoardEnt(name);

            await _boardRepository.AddAsync(board);
            await _boardRepository.SaveChangesAsync();

            return board;
        }
    }
}
