using DevBoard.Domain.Boards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Application.Boards.Interfaces
{
    public interface IBoardService
    {
        Task<BoardEnt> CreateBoardAsync(string name);
    }
}
