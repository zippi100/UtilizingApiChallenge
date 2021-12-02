using Newtonsoft.Json;

namespace Coding_Challenge.Models
{
    public class AnswerResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
        
        [JsonProperty("totalMilliseconds")]
        public int TotalMilliseconds { get; set; }
    }
}
