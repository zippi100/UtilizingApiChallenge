using Newtonsoft.Json;

namespace Coding_Challenge.Models
{
    public class VehicleAnswer
    {
        public VehicleAnswer() { }
        public VehicleAnswer(int vehicleId, int year, string make, string model)
        {
            VehicleId = vehicleId;
            Year = year;
            Make = make;
            Model = model;
        }
        
        [JsonProperty("vehicleId")]
        public int VehicleId{ get; set; }
        [JsonProperty("year")]
        public int Year { get; set; }
        [JsonProperty("make")]
        public string Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
    }
}
