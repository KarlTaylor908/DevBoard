using DevBoard.Domain.Auth.Entities;
using DevBoard.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Boards.Entities
{
    public class BoardEnt : BaseEnt
    {
        public BoardEnt(string name)
        {
            Name = name.ToLower();

        }
        public string Name { get; set; }

        public Guid? UserId { get; set; }
        public UserEnt? User { get; set; }

    }
}
