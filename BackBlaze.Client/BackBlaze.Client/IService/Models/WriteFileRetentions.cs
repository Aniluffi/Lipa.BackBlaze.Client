using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models
{
    public class WriteFileRetentions
    {
        public string mode { get; set; }

        public int retainUntilTimestamp { get; set; }
    }
}
