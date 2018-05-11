using System;
using System.Collections.Generic;
using System.Text;
using InstaSharper.Classes.Models;
using Newtonsoft.Json;
namespace InstaSharper.Classes
{
    public class BroadcastSuggestedResponse
    {
        [JsonProperty("broadcasts")]
        public List<InstaBroadcast> Broadcasts { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
