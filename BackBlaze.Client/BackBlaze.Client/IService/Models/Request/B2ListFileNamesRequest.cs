using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Request
{
    public class B2ListFileNamesRequest
    {
        public string bucketId { get; set; }
        public string startFileName { get; set; }
        public int maxFileCount { get; set; }
        public string prefix { get; set; }

        public string delimiter { get; set; }
    }
}
