using Newtonsoft.Json;
using System.Collections.Generic;

namespace Coding_Challenge.Models
{

    public class Answer: DealerAnswer
    {
        public Answer() 
        {
            Dealers = new List<DealerAnswer>();
        }
  

        [JsonProperty("dealers")]
        public List<DealerAnswer> Dealers { get; set;}
    }
}
