using Newtonsoft.Json;
using System.Collections.Generic;

namespace Coding_Challenge.Models
{
    public class VehicleIdsResponse
    {
        [JsonProperty("vehicleIds")]
        public List<int> VehicleIds { get; set; }
    }
}
