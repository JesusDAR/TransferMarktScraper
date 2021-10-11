using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransferMarktScraper.WebApi
{
    public static class Constants
    {
        public static string SaisonId = "/" + DateTime.Today.Year.ToString();
        
        public static readonly string Transfermarkt = "https://www.transfermarkt.es";
        public static readonly string LaLiga = "/laliga/startseite/wettbewerb/ES1";
        public static readonly string Ampliado = "/plus/1";

        public enum Code
        {
            Success = 0,
            Error = 1
        }
        //public static readonly string AmpliadoPerformance = "//saison/verein/0/liga/0/wettbewerb//pos/0/trainer_id/0/plus/1";

    }
}
