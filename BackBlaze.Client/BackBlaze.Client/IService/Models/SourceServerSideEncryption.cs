using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models
{
    public class SourceServerSideEncryption
    {
        public string mode { get; set; }
        public string algorithm { get; set; }

        public string customerKey { get; set; }

        public string customerKeyMd5 { get; set; }
    }
}
