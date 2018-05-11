using InstaSharper.Classes;
using InstaSharper.Classes.Android.DeviceInfo;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using InstaSharper.Helpers;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace InstaSharper.API.Processors
{
    internal class LiveProcessor : ILiveProcessor
    {
        private readonly AndroidDevice _deviceInfo;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly IInstaLogger _logger;
        private readonly UserSessionData _user;

        public LiveProcessor(AndroidDevice deviceInfo, UserSessionData user,
            IHttpRequestProcessor httpRequestProcessor, IInstaLogger logger)
        {
            _deviceInfo = deviceInfo;
            _user = user;
            _httpRequestProcessor = httpRequestProcessor;
            _logger = logger;
        }

        public async Task<IResult<BroadcastLiveHeartBeatViewerCountResponse>> GetHeartBeatAndViewerCountAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/heartbeat_and_get_viewer_count/");
                Debug.WriteLine(instaUri.ToString());

                var uploadId = ApiRequestMessage.GenerateUploadId();
                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    {new StringContent(_user.CsrfToken), "\"_csrftoken\""},
                    {new StringContent(_deviceInfo.DeviceGuid.ToString()), "\"_uuid\""},
                    {new StringContent("offset_to_video_start"),"30"}
                };
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, _deviceInfo);
                request.Content = requestContent;
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastLiveHeartBeatViewerCountResponse>(json);
                //{"viewer_count": 0.0, "broadcast_status": "interrupted", "cobroadcaster_ids": [], "offset_to_video_start": 0, "total_unique_viewer_count": 0, "is_top_live_eligible": 0, "status": "ok"}
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastLiveHeartBeatViewerCountResponse>(exception);
            }
        }
        
        public async Task<IResult<BroadcastFinalViewerListResponse>> GetFinalViewerListAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/get_final_viewer_list/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastFinalViewerListResponse>(json);
                //{"users": [], "total_unique_viewer_count": 0, "status": "ok"}
                return Result.Success(obj);

            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastFinalViewerListResponse>(exception);
            }
        }

        public async Task<IResult<object>> NotifyToFriendsAsync()
        {
            
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + "live/get_live_presence/?presence_type=30min");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadCastNotifyFriendsResponse>(json);
                //{"text": "We'll notify some of your followers so they don't miss it.", "friends": [], "online_friends_count": 0, "status": "ok"}
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadCastNotifyFriendsResponse>(exception);
            }
        }
        
        public async Task<IResult<object>> SeenBroadcastAsync(string broadcastId, string pk)
        {
            try
            {
                Debug.WriteLine("URL: " +InstaApiConstants.BASE_INSTAGRAM_API_URL);
                var instaUri = new Uri($"https://i.instagram.com/api/v2/media/seen/?reel=1&live_vod=0");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    {"_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"live_vods_skipped",  new JObject()},
                    {"nuxes_skipped",  new JObject()},
                    {"nuxes",  new JObject()},
                    {"reels",  new JObject{ { broadcastId, new JArray(pk) } } },
                    {"live_vods",  new JObject()},
                    {"reel_media_skipped",  new JObject()},

                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                //{"users": [], "total_unique_viewer_count": 0, "status": "ok"}
                return Result.Success(json);

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                _logger?.LogException(exception);
                return Result.Fail<BroadcastLiveHeartBeatViewerCountResponse>(exception);
            }
        }
        
        public async Task<IResult<BroadcastSuggestedResponse>> GetSuggestedBroadcastsAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + "live/get_suggested_broadcasts/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastSuggestedResponse>(json);

                
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastSuggestedResponse>(exception);
            }
        }

        public async Task<IResult<DiscoverTopLiveResponse>> GetDiscoverTopLiveAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + "discover/top_live/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DiscoverTopLiveResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DiscoverTopLiveResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastTopLiveStatusResponse>> GetTopLiveStatusAsync(params string[] broadcastIds)
        {
            if(broadcastIds == null)
                return Result.Fail<BroadcastTopLiveStatusResponse>("broadcast ids must be set");
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + "discover/top_live_status/");
                Debug.WriteLine(instaUri.ToString());
                var data = new JObject
                {
                    {"broadcast_ids", new JArray(broadcastIds)},
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                // age broadcast tamom shode bashe
                // "broadcast_status": "hard_stop"
                // age broadcast dar hale live bashe:
                // "broadcast_status": "post_live"
                var obj = JsonConvert.DeserializeObject<BroadcastTopLiveStatusResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastTopLiveStatusResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastInfoResponse>> GetInfoAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/info/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastInfoResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastInfoResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastViewerListResponse>> GetViewerListAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/get_viewer_list/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastViewerListResponse>(json);
                //{"users": [], "status": "ok"}
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastViewerListResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastViewerListResponse>> GetPostLiveViewerListAsync(string broadcastId, int? maxId = null)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/get_post_live_viewers_list/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                //{"message": "Not broadcast owner", "status": "fail"}
                //{"users": [], "next_max_id": null, "total_viewer_count": 0, "status": "ok"}
                var obj = JsonConvert.DeserializeObject<BroadcastViewerListResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastViewerListResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastSendCommentResponse>> CommentAsync(string broadcastId, string commentText)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/comment/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    {"user_breadcrumb", commentText},
                    {"idempotence_token", _deviceInfo.DeviceGuid.ToString()},
                    {"comment_text", commentText},
                    {"live_or_vod", 1},
                    {"offset_to_video_start", 0}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                //{"message": "This broadcast is already ended, your comment is not displayed.", "status": "fail"}
                //{"comment": {"content_type": "comment", "user": {"pk": 1647718432, "username": "kajokoleha", "full_name": "kajokoleha", "has_anonymous_profile_picture": false, "is_private": false, "is_verified": false, "profile_pic_url": "https://instagram.fgyd4-2.fna.fbcdn.net/vp/e66040abb81eead93498fdb5e3501b7b/5B820059/t51.2885-19/s150x150/29094366_375967546140243_535690319979610112_n.jpg", "profile_pic_id": "1746518311616597634_1647718432", "allowed_commenter_type": "any", "reel_auto_archive": "unset"}, "pk": 17930083636109264, "text": "test123", "type": 0, "created_at": 1526022240.0, "created_at_utc": 1526051040, "media_id": 1776719255547726806, "status": "Active"}, "status": "ok"}

                var obj = JsonConvert.DeserializeObject<BroadcastSendCommentResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastSendCommentResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastPinUnpinResponse>> PinCommentAsync(string broadcastId, string commentId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/pin_comment/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"comment_id", commentId},
                    {"offset_to_video_start", 0}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                //{"comment_id": 17943751696013041, "status": "ok"}
                var obj = JsonConvert.DeserializeObject<BroadcastPinUnpinResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastPinUnpinResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastPinUnpinResponse>> UnPinCommentAsync(string broadcastId, string commentId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/unpin_comment/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"comment_id", commentId},
                    {"offset_to_video_start", 0}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastPinUnpinResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastPinUnpinResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastCommentResponse>> GetCommentsAsync(string broadcastId, int lastCommentTs = 0, int commentsRequested =4)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/get_comment/");
                Debug.WriteLine(instaUri.ToString());

                Dictionary<string, int> data = new Dictionary<string, int>
                {
                    { "last_comment_ts", lastCommentTs },
                    { "num_comments_requested", commentsRequested }
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Get, instaUri, _deviceInfo, data);
                //var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastCommentResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastCommentResponse>(exception);
            }
        }

        public async Task<IResult<object>> GetPostLiveCommentsAsync(string broadcastId, int startingOffset = 0, string encodingTag = "instagram_dash_remuxed")
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/get_post_live_comments/?starting_offset={startingOffset}&encoding_tag={encodingTag}");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadCastNotifyFriendsResponse>(json);

                return Result.Success(json);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<string>(exception);
            }
        }

        public async Task<IResult<BroadcastCommentEnableDisableResponse>> EnableCommentsAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/unmute_comment/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                //{"comment_muted": 0, "status": "ok"}
                var obj = JsonConvert.DeserializeObject<BroadcastCommentEnableDisableResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastCommentEnableDisableResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastCommentEnableDisableResponse>> DisableCommentsAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/mute_comment/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                //{"comment_muted": 1, "status": "ok"}
                var obj = JsonConvert.DeserializeObject<BroadcastCommentEnableDisableResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastCommentEnableDisableResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastLikeResponse>> LikeAsync(string broadcastId, int likeCount = 1)
        {
            //if (likeCount < 1 || likeCount > 6)
            //    Result.Fail<BroadcastLikeResponse>("Like count must be a number from 1 to 6.");
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/like/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"user_like_count", likeCount}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastLikeResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastLikeResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastLikeResponse>> GetLikeCountAsync(string broadcastId, int likeTs = 0)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/get_like_count/?like_ts={likeTs}");
                Debug.WriteLine(instaUri.ToString());

                Dictionary<string, int> data = new Dictionary<string, int>
                {
                    { "like_ts", likeTs }
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Get, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastLikeResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastLikeResponse>(exception);
            }
        }

        public async Task<IResult<object>> GetPostLiveLikesAsync(string broadcastId, int startingOffset = 0, string encodingTag = "instagram_dash_remuxed")
        {
            try
            {
                // kamel nist
                //{"message": "starting_offset param not sent", "status": "fail"}

                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/get_post_live_likes/?starting_offset={startingOffset}&encoding_tag={encodingTag}");
                Debug.WriteLine(instaUri.ToString());

                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "starting_offset", startingOffset },
                    { "encoding_tag", encodingTag }
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Get, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadCastNotifyFriendsResponse>(json);
                //{"message": "starting_offset param not sent", "status": "fail"}
                return Result.Success(json);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<string>(exception);
            }
        }

        public async Task<IResult<BroadcastAddToPostLiveResponse>> AddToPostLiveAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/add_to_post_live/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastAddToPostLiveResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastAddToPostLiveResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastDefaultResponse>> DeletePostLiveAsync(string broadcastId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/delete_post_live/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastDefaultResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastDefaultResponse>(exception);
            }
        }


        // create, start, end broadcast

        public async Task<IResult<BroadcastCreateResponse>> CreateAsync(int previewWidth = 720, int previewHeight = 1184, string broadcastMessage = "")
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/create/");
                Debug.WriteLine(instaUri.ToString());
                var data = new JObject
                {
                    {"_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"preview_height",  previewHeight},
                    {"preview_width",  previewWidth},
                    {"broadcast_message",  broadcastMessage},
                    {"broadcast_type",  "RTMP"},
                    {"internal_only",  0},

                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo,data);
                request.Headers.Host = "i.instagram.com";

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastCreateResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastCreateResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastStartResponse>> StartAsync(string broadcastId, bool sendNotifications)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/start/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    {"_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"should_send_notifications",  sendNotifications}

                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastStartResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastStartResponse>(exception);
            }
        }

        public async Task<IResult<BroadcastDefaultResponse>> EndAsync(string broadcastId, bool endAfterCopyrightWarning = false)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"live/{broadcastId}/end_broadcast/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    {"_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.UserName},
                    {"end_after_copyright_warning", "false"},
                };

            
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastDefaultResponse>(json);
                //{"status": "ok"}
                return Result.Success(obj);

            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<BroadcastDefaultResponse>(exception);
            }
        }

    }
}
