using DevBoard.Domain.Boards.Entities;
using DevBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Boards
{
    public interface IBoardRepository : IBaseRepository<BoardEnt>
    {
    }
}
