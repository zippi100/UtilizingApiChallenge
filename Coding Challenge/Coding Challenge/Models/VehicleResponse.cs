using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coding_Challenge.Models
{
    public class VehicleResponse
    {
        [JsonProperty("vehicleId")]
        public int VehicleId { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("make")]
        public string Make { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
    }
}
