using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Http.Client.Options
{
    public class HttpClientOptions
    {
        public int TimeoutMs { get; set; } = 10000;

        public long RetryCount { get; set; } = 2;

        public long RetryDelayMs { get; set; } = 1000;
    }
}
