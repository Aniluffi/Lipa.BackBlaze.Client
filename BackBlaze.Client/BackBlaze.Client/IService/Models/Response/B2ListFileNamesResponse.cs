using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackBlaze.Client.IService.Models.Response
{
    public class B2ListFileNamesResponse
    {
        public List<B2File> files { get; set; }

        public string nextFileName { get; set; }
    }
}
