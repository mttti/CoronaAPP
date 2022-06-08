using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoronaAPP
{
    public class Root
    {
        public string Country_text { get; set; }

        [JsonProperty("Last Update")]
        public string LastUpdate { get; set; }

        [JsonProperty("Total Cases_text")]
        public string TotalCasesText { get; set; }

        [JsonProperty("Total Deaths_text")]
        public string TotalDeathsText { get; set; }

    }
}
