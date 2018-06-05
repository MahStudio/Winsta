using InstaSharper.Classes.Models;
using Newtonsoft.Json;

namespace InstaSharper.Classes.ResponseWrappers
{
    public class InstaCommentResponse
    {
        [JsonProperty("type")] public int Type { get; set; }

        [JsonProperty("bit_flags")] public int BitFlags { get; set; }

        [JsonProperty("user_id")] public long UserId { get; set; }

        [JsonProperty("status")] public string Status { get; set; }

        [JsonProperty("created_at_utc")] public string CreatedAtUtc { get; set; }

        [JsonProperty("comment_like_count")] public int LikesCount { get; set; }

        [JsonProperty("created_at")] public string CreatedAt { get; set; }

        [JsonProperty("content_type")] public string ContentType { get; set; }

        [JsonProperty("user")] public InstaUserShortResponse User { get; set; }

        [JsonProperty("pk")] public long Pk { get; set; }

        [JsonProperty("text")] public string Text { get; set; }

        [JsonProperty("did_report_as_spam")] public bool DidReportAsSpam { get; set; }

        [JsonProperty("has_liked_comment")] public bool HasLikedComment { get; set; }

        [JsonProperty("child_comment_count")] public int ChildCommentCount { get; set; }

        [JsonProperty("num_tail_child_comments")] public int NumTailChildComments { get; set; }

        [JsonProperty("has_more_tail_child_comments")] public bool HasMoreTailChildComments { get; set; }

        [JsonProperty("has_more_head_child_comments")] public bool HasMoreHeadChildComments { get; set; }

        [JsonProperty("next_max_child_cursor")] public string NextMaxChildCursor { get; set; }

        //[JsonProperty("preview_child_comments")] public InstaCommentList PreviewChildComments { get; set; }

        //"next_max_child_cursor": "AQC5sb1eLI83uSipN6uwfM6fFBaoKY4CxmmxJfHe7PzGCc_k3ynU6cY0tQT2ps6vO4XprYotxT8eXRvNham3z5gWrlGW08xiG-szC538GSjHsw",
        //"num_tail_child_comments": 10,

        //"did_report_as_spam": false,
        //"has_liked_comment": false,
        //"child_comment_count": 13,
        //"num_tail_child_comments": 13,
        //"has_more_tail_child_comments": true,
        //"has_more_head_child_comments": false,
        //"inline_composer_display_condition": "never"




        //"preview_child_comments": [],
    }
}