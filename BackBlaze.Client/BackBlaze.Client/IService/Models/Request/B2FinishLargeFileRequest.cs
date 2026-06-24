using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Request
{
    public class B2FinishLargeFileRequest
    {
        public string fileId { get; set; }  

        public List<string> partSha1Array { get; set; }
    }
}
