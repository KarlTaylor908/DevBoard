using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBoard.Domain.Shared
{
    public class BaseEnt
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
