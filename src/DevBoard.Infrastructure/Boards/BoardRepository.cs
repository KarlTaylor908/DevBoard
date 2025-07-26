using DevBoard.Domain.Boards;
using DevBoard.Domain.Boards.Entities;
using DevBoard.Infrastructure.Data;
using DevBoard.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Infrastructure.Boards
{
    public class BoardRepository : BaseRepository<BoardEnt>, IBoardRepository
    {
        private readonly AppDbContext _db;

        public BoardRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
