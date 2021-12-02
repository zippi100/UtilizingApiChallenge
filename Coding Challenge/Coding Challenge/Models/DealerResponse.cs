using Newtonsoft.Json;

namespace Coding_Challenge.Models
{
    public class DealerResponse
    {
        [JsonProperty("dealerId")]
        public int DealerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
