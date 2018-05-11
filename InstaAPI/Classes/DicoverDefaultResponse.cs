using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InstaSharper.Classes
{
    public class DicoverDefaultResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}