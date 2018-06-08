using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaAPI
{

    public class NotifyList : List<NotifyClass>
    {
        public bool IsExists(string text)
        {
            return this.Any(x => x.Text.ToLower().StartsWith(text.ToLower()));
        }
        public int GetIndex(string text)
        {
            return this.FindIndex(x => x.Text.ToLower().StartsWith(text.ToLower()));
        }
    }
    //"ActivityFeed": {
    //    "ProfileId": 7051302384,
    //    "ProfileImage": "https://scontent-frt3-1.cdninstagram.com/vp/4a0977cee0757b309fab1797fb91d167/5BC1BA93/t51.2885-19/s150x150/27581171_1344917458945556_1773829597851287552_n.jpg",
    //    "TimeStamp": "1970-01-01T00:25:27",
    //    "Text": "soroush._mp_ and fanscape liked your post.",
    //    "Links": [
    //        {
    //            "Type": "user",
    //            "Start": "0",
    //            "End": "12",
    //            "Id": "7051302384"
    //        },
    //        {
    //            "Type": "user",
    //            "Start": "17",
    //            "End": "25",
    //            "Id": "4059329432"
    //        }
    //    ],
    //    "InlineFollow": null,
    //    "Type": 1,
    //    "Pk": "162fe2d8b116cad7b0301b1052bde290"
    //}
    public class NotifyClass
    {
        public bool IsShowing { get; set; }
        public string Text { get; set; }

        public string Username { get; set; }
        public long ProfileId { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsFollowingYou { get; set; }
        public string TimeStamp { get; set; }
        public int Type { get; set; }
        //public InstaRecentActivityFeed ActivityFeed { get; set; }
   
    }
}
