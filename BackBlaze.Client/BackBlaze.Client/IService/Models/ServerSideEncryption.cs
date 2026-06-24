using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models
{
    public class ServerSideEncryption
    {
        public string? algorithm { get; set; }

        public string? mode { get; set; }
    }
}
