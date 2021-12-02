using Newtonsoft.Json;
using System.Collections.Generic;

namespace Coding_Challenge.Models
{
    public class DealerAnswer: VehicleAnswer
    {
        public DealerAnswer() { }
        public DealerAnswer(int dealerId, string name)
        {
            DealerId = dealerId;
            Name = name;
            Vehicles = new List<VehicleAnswer>();
        }

        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vehicles")]
        //public VehicleAnswer Vehicles { get; set; }
        public List<VehicleAnswer> Vehicles { get; set; }
    }
}
