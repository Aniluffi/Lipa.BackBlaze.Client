using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models
{
    public class FileRetention
    {
        public bool isClientAuthorizedToRead { get; set; }

        public Value value { get; set; }
    }
}
