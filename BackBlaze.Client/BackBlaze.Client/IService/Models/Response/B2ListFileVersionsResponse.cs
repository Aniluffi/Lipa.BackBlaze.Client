using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class B2ListFileVersionsResponse
    {
        public List<B2File> files { get; set; }

        public string nextFileName { get; set; }

        public string nextFileId { get; set; }
    }
}
