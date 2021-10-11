using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransferMarktScraper.WebApi.DTOs
{
    public class ScrapeResults
    {
        public List<ScrapeResult> Results { get; set; }
    }

    public class ScrapeResult
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }
}
