using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace InstaSharper.Classes
{

    public class BroadcastPinUnpinResponse
    {
        [JsonProperty("comment_id")]
        public long CommentId { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

}
