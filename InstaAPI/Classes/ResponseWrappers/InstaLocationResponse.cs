using Newtonsoft.Json;

namespace InstaSharper.Classes.ResponseWrappers
{
    public class InstaLocationResponse : InstaLocationShortResponse
    {
        [JsonProperty("facebook_places_id")] public long FacebookPlacesId { get; set; }

        [JsonProperty("city")] public string City { get; set; }

        [JsonProperty("pk")] public long Pk { get; set; }

        [JsonProperty("short_name")] public string ShortName { get; set; }
    }
    public class InstaLocationStoryResponse
    {
        [JsonProperty("x")] public float X { get; set; }
        [JsonProperty("y")] public float Y { get; set; }
        [JsonProperty("z")] public int Z { get; set; }
        [JsonProperty("width")] public float Width { get; set; }
        [JsonProperty("height")] public float Height { get; set; }
        [JsonProperty("rotation")] public float Rotation { get; set; }
        [JsonProperty("is_pinned")] public int IsPinned { get; set; }
        [JsonProperty("is_hidden")] public int IsHidden { get; set; }
        [JsonProperty("location")] public InnerLocationResp Location { get; set; }
    }

    public class InnerLocationResp
    {
        [JsonProperty("pk")] public long Pk { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("address")] public string Address { get; set; }
        [JsonProperty("city")] public string City { get; set; }
        [JsonProperty("short_name")] public string ShortName { get; set; }
        [JsonProperty("lng")] public double Lng { get; set; }
        [JsonProperty("lat")] public double Lat { get; set; }
        [JsonProperty("external_source")] public string ExternalSource { get; set; }
        [JsonProperty("facebook_places_id")] public long FacebookPlacesId { get; set; }
    }
    //public class InstaLocationResponse : InstaLocationShortResponse
    //{
    //    [JsonProperty("facebook_places_id")] public long FacebookPlacesId { get; set; }

    //    [JsonProperty("city")] public string City { get; set; }

    //    [JsonProperty("pk")] public long Pk { get; set; }

    //    [JsonProperty("short_name")] public string ShortName { get; set; }
    //}
}