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
using System.Net.Sockets;
using InstaSharper.Converters;
using InstaSharper.Classes.ResponseWrappers;
using InstaSharper.Classes.Models;
using System.Net;

namespace InstaSharper.API.Processors
{
    internal class AccountProcessor : IAccountProcessor
    {
        private readonly AndroidDevice _deviceInfo;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly IInstaLogger _logger;
        private readonly UserSessionData _user;

        public AccountProcessor(AndroidDevice deviceInfo, UserSessionData user,
            IHttpRequestProcessor httpRequestProcessor, IInstaLogger logger)
        {
            _deviceInfo = deviceInfo;
            _user = user;
            _httpRequestProcessor = httpRequestProcessor;
            _logger = logger;
        }

        /// <summary>
        /// NOT COMPLETE
        /// </summary>
        /// <param name="bio"></param>
        /// <returns></returns>
        public async Task<IResult<object>> SetBiographyAsync(string bio)
        {
            try
            {
                //POST /api/v1/accounts/set_biography/ HTTP/1.1

                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/set_biography/");
                Debug.WriteLine(instaUri.ToString());
                
                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    { "raw_text", bio}
                };
                Debug.WriteLine("-----------------------");
                Debug.WriteLine(JsonConvert.SerializeObject(data));
                Debug.WriteLine("--");

                Debug.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));

                Debug.WriteLine("-----------------------");
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(response.StatusCode);
                // hamash NotFound return mikone:|
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (BroadcastCommentResponse)null);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<BroadcastCommentResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                _logger?.LogException(exception);
                return Result.Fail<BroadcastCommentResponse>(exception);
            }
        }

        public async Task<IResult<AccountUserResponse>> EditProfileAsync(string url, string phone, string name, string biography, string email, GenderType gender, string newUsername = null)
        {
            try
            {
                var editRequest = await GetRequestForEditProfileAsync();
                if(!editRequest.Succeeded)
                    return Result.Fail("Edit request returns badrequest", (AccountUserResponse)null);
                var user = editRequest.Value.User.Username;

                if (string.IsNullOrEmpty(newUsername))
                    newUsername = user;
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/edit_profile/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    {"external_url", url},
                    {"gender", ((int)gender).ToString()},
                    {"phone_number", phone},
                    {"_csrftoken", _user.CsrfToken},
                    {"username", newUsername},
                    {"first_name", name},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"biography", biography},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"email", email},
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountUserResponse)null);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountUserResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountUserResponse>(exception);
            }
        }
        
        public async Task<IResult<AccountUserResponse>> GetRequestForEditProfileAsync()
        {
            try
            {

                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/current_user/?edit=true");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountUserResponse)null);
                Debug.WriteLine(json);

                var o = JsonConvert.DeserializeObject<AccountUserResponse>(json);
                
                return Result.Success(o);
            
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountUserResponse>(exception);
            }
        }

        public async Task<IResult<bool>> SetNameAndPhoneNumberAsync(string name, string phoneNumber = "")
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/set_phone_and_name/");
                Debug.WriteLine(instaUri.ToString());
                var data = new JObject
                {
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    { "_csrftoken", _user.CsrfToken},
                    {"first_name", name},
                    {"phone_number", phoneNumber}
                };

                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();               

                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                Debug.WriteLine(json);
                //{"status": "ok"}
                var obj = JsonConvert.DeserializeObject<AccountDefaultResponse>(json);
                if (obj.Status.ToLower() == "ok")
                    return Result.Success(true);
                else
                    return Result.Success(false);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }

        public async Task<IResult<AccountUserResponse>> RemoveProfilePictureAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/remove_profile_picture/");
                Debug.WriteLine(instaUri.ToString());
                var data = new JObject
                {
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    { "_csrftoken", _user.CsrfToken}
                };

                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Add("Host", "i.instagram.com");
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountUserResponse)null);
                Debug.WriteLine(json);
                //{"user": {"pk": 7560977630, "username": "sepideh652ansari943499", "full_name": "Sepideh Ansari", "has_anonymous_profile_picture": true, "is_private": false, "is_verified": false, "profile_pic_url": "https://scontent-dfw5-1.cdninstagram.com/vp/856b9478629f7c2f4ae549c4c8cc5dd7/5B94597A/t51.2885-19/11906329_960233084022564_1448528159_a.jpg", "allowed_commenter_type": "any", "biography": "S\nAr\n salam\nkhobi?", "external_url": "https://t.me/r1234566", "external_lynx_url": "https://l.instagram.com/?u=https%3A%2F%2Ft.me%2Fr1234566\u0026e=ATN3tDdL99TY1tUd6eW92xohcXBWML4yshGPayPBdb6E_cXUVjr_CQPcWpH2a39G1BSCUu7Cuj-5iluG", "hd_profile_pic_url_info": {"height": 150, "url": "https://scontent-dfw5-1.cdninstagram.com/vp/856b9478629f7c2f4ae549c4c8cc5dd7/5B94597A/t51.2885-19/11906329_960233084022564_1448528159_a.jpg", "width": 150}, "hd_profile_pic_versions": [{"height": 320, "url": "https://scontent-dfw5-1.cdninstagram.com/vp/856b9478629f7c2f4ae549c4c8cc5dd7/5B94597A/t51.2885-19/11906329_960233084022564_1448528159_a.jpg", "width": 320}, {"height": 640, "url": "https://scontent-dfw5-1.cdninstagram.com/vp/856b9478629f7c2f4ae549c4c8cc5dd7/5B94597A/t51.2885-19/11906329_960233084022564_1448528159_a.jpg", "width": 640}], "reel_auto_archive": "unset"}, "status": "ok"}

                var obj = JsonConvert.DeserializeObject<AccountUserResponse>(json);

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                _logger?.LogException(exception);
                return Result.Fail<AccountUserResponse>(exception);
            }
        }

        public async Task<IResult<AccountUserResponse>> ChangeProfilePictureAsync(byte[] pictureBytes)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/change_profile_picture/");
                Debug.WriteLine(instaUri.ToString());
                var uploadId = ApiRequestMessage.GenerateUploadId();
                var requestContent = new MultipartFormDataContent(uploadId)
                {
                    {new StringContent(_deviceInfo.DeviceGuid.ToString()), "\"_uuid\""},
                    {new StringContent(_user.LoggedInUder.Pk.ToString()), "\"_uid\""},
                    {new StringContent(_user.CsrfToken), "\"_csrftoken\""}
                };
                var imageContent = new ByteArrayContent(pictureBytes);
                requestContent.Add(imageContent, "profile_pic", $"r{ApiRequestMessage.GenerateUploadId()}.jpg");
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, _deviceInfo);
                request.Content = requestContent;
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountUserResponse)null);
                Debug.WriteLine(json);
                //{"user": {"pk": 7560977630, "username": "sepideh652ansari943499", "full_name": "Sepideh Ansari", "has_anonymous_profile_picture": false, "is_private": false, "is_verified": false, "profile_pic_url": "https://scontent-frt3-2.cdninstagram.com/vp/c50253e0f77b50c728302071546f5245/5B8CC2EE/t51.2885-19/s150x150/32039836_835196020005106_5766236187517779968_n.jpg", "profile_pic_id": "1778512735216974581_7560977630", "allowed_commenter_type": "any", "biography": "S\nAr\n salam\nkhobi?", "external_url": "https://t.me/r1234566", "external_lynx_url": "https://l.instagram.com/?u=https%3A%2F%2Ft.me%2Fr1234566\u0026e=ATMfpUlhkVCWHq8VYuNRLj5ff8g9XYoWnBSR1igXqXFYGyKWhO8I8J6jna_N1leZjqvUQYB3KJzskHtX", "hd_profile_pic_url_info": {"height": 768, "url": "https://scontent-frt3-2.cdninstagram.com/vp/76b1bcd3678c3c7e65deacc17456160b/5B8DF014/t51.2885-19/32039836_835196020005106_5766236187517779968_n.jpg", "width": 768}, "hd_profile_pic_versions": [{"height": 320, "url": "https://scontent-frt3-2.cdninstagram.com/vp/3977e4a672badb5e7f049e995083d2b2/5B9CEC1E/t51.2885-19/s320x320/32039836_835196020005106_5766236187517779968_n.jpg", "width": 320}, {"height": 640, "url": "https://scontent-frt3-2.cdninstagram.com/vp/cf4e54c4f65094e6fbe1d1d8348ff5be/5B7C1071/t51.2885-19/s640x640/32039836_835196020005106_5766236187517779968_n.jpg", "width": 640}], "reel_auto_archive": "unset"}, "status": "ok"}




                //from insta web returns:
                //{"changed_profile": false, "message": "Sorry, this picture format isn't supported. Please try another picture in JPEG format.", "status": "ok"}
                //{"changed_profile": true, "id": 7560977630, "has_profile_pic": true, "profile_pic_url": "https://scontent-frt3-2.cdninstagram.com/vp/83d757e2e68302b73cb8683eb53e1491/5B99150F/t51.2885-19/s150x150/31439599_310472246152089_1272586375874478080_n.jpg", "profile_pic_url_hd": "https://scontent-frt3-2.cdninstagram.com/vp/b2eba51b76de5704338b542777ce67bf/5B962DFF/t51.2885-19/s320x320/31439599_310472246152089_1272586375874478080_n.jpg", "status": "ok"}

                var obj = JsonConvert.DeserializeObject<AccountUserResponse>(json);
       
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                _logger?.LogException(exception);
                return Result.Fail<AccountUserResponse>(exception);
            }
        }




        // Story settings
        public async Task<IResult<AccountSettingsResponse>> GetStorySettingsAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/reel_settings/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountSettingsResponse)null);
                Debug.WriteLine(json);
                //{"message_prefs": "off", "blocked_reels": {"users": [], "big_list": false, "page_size": 200}, "besties": {"users": [], "big_list": false, "page_size": 200}, "persist_stories_to_private_profile": true, "reel_auto_archive": "on", "allow_story_reshare": true, "save_to_camera_roll": false, "status": "ok"}

                var obj = JsonConvert.DeserializeObject<AccountSettingsResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountSettingsResponse>(exception);
            }
        }


        public async Task<IResult<bool>> EnableSaveStoryToGalleryAsync()
        {
            try
            {
                // POST /api/v1/ HTTP/1.1                
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/set_reel_settings/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"save_to_camera_roll", 1.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountArchiveStoryResponse>(json);
                //{"message_prefs": null, "status": "ok"}
                if (obj.Status.ToLower() == "ok")
                    return Result.Success(true);
                else
                    return Result.Success(false);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }

        public async Task<IResult<bool>> DisableSaveStoryToGalleryAsync()
        {
            try
            {
                // POST /api/v1/ HTTP/1.1
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/set_reel_settings/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"save_to_camera_roll", 0.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountArchiveStoryResponse>(json);
                //{"message_prefs": null, "status": "ok"}
                if (obj.Status.ToLower() == "ok")
                    return Result.Success(true);
                else
                    return Result.Success(false);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }


        public async Task<IResult<bool>> EnableSaveStoryToArchiveAsync()
        {
            try
            {
                //POST /api/v1/users/set_reel_settings/ HTTP/1.1
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/set_reel_settings/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"reel_auto_archive", "on"}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountArchiveStoryResponse>(json);
                //{"reel_auto_archive": "on", "message_prefs": null, "status": "ok"}
                if (obj.ReelAutoArchive.ToLower() == "on")
                    return Result.Success(true);
                else
                    return Result.Success(false);

            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }

        public async Task<IResult<bool>> DisableSaveStoryToArchiveAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/set_reel_settings/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"check_pending_archive", "1"},
                    {"reel_auto_archive", "off"}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountArchiveStoryResponse>(json);
                //{ "reel_auto_archive": "off", "message_prefs": null, "status": "ok"}
                if(obj.ReelAutoArchive.ToLower() == "off")
                    return Result.Success(true);
                else
                    return Result.Success(false);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }


        public async Task<IResult<bool>> AllowStorySharingAsync(bool allow = true)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/set_reel_settings/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                };
                if (allow)
                    data.Add("allow_story_reshare", "1");
                else
                    data.Add("allow_story_reshare", "0");
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountArchiveStoryResponse>(json);
                //{"message_prefs": null, "status": "ok"}
                if (obj.Status.ToLower() == "off")
                    return Result.Success(true);
                else
                    return Result.Success(false);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }
        
        public async Task<IResult<bool>> AllowStoryMessageRepliesAsync(MessageRepliesType repliesType)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/set_reel_settings/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                };
                switch (repliesType)
                {
                    case MessageRepliesType.Everyone:
                        data.Add("message_prefs", "anyone");
                        break;
                    case MessageRepliesType.Following:
                        data.Add("message_prefs", "following");
                        break;
                    case MessageRepliesType.Off:
                        data.Add("message_prefs", "off");
                        break;
                }
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                //{"message_prefs": "anyone", "status": "ok"}
                //{"message_prefs": "following", "status": "ok"}
                //{"message_prefs": "anyone", "status": "ok"}
                var obj = JsonConvert.DeserializeObject<AccountArchiveStoryResponse>(json);


                if (obj.MessagePrefs.ToLower() == "anyone" && repliesType == MessageRepliesType.Everyone)
                    return Result.Success(true);
                else if (obj.MessagePrefs.ToLower() == "following" && repliesType == MessageRepliesType.Following)
                    return Result.Success(true);
                else if (obj.MessagePrefs.ToLower() == "off" && repliesType == MessageRepliesType.Off)
                    return Result.Success(true);
                else
                    return Result.Success(false);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }


        public async Task<IResult<AccountCheckResponse>> CheckUsernameAsync(string desiredUsername)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/check_username/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"username", desiredUsername}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountCheckResponse)null);
                var obj = JsonConvert.DeserializeObject<AccountCheckResponse>(json);
                //{"username": "rmt", "available": false, "error": "The username rmt is not available.", "status": "ok", "error_type": "username_is_taken"}
                //{"username": "rmt40066", "available": true, "status": "ok"}


                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountCheckResponse>(exception);
            }
        }




        // two factor authentication enable/disable
        public async Task<IResult<bool>> DisableTwoFactorAuthenticationAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/disable_sms_two_factor/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, false);
                var obj = JsonConvert.DeserializeObject<AccountCheckResponse>(json);
                //{"status": "ok"}


                if (obj.Status.ToLower() == "ok")
                    return Result.Success(true);
                else
                    return Result.Success(false);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }

        public async Task<IResult<AccountTwoFactorSmsResponse>> SendTwoFactorEnableSmsAsync(string phoneNumber)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/send_two_factor_enable_sms/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    { "device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    { "phone_number", phoneNumber},

                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountTwoFactorSmsResponse)null);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountTwoFactorSmsResponse>(json);
                //{"phone_verification_settings": {"max_sms_count": 2, "resend_sms_delay_sec": 60, "robocall_after_max_sms": false, "robocall_count_down_time_sec": 30}, "obfuscated_phone_number": "4006", "status": "ok"}

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountTwoFactorSmsResponse>(exception);
            }
        }

        public async Task<IResult<AccountTwoFactorResponse>> TwoFactorEnableAsync(string phoneNumber, string verificationCode)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/enable_sms_two_factor/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    { "device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    { "phone_number", phoneNumber},
                    { "verification_code", verificationCode}

                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountTwoFactorResponse)null);
                var obj = JsonConvert.DeserializeObject<AccountTwoFactorResponse>(json);
                //{"backup_codes": ["1396 5240", "9027 8541", "6824 5907", "8760 3519", "5293 4608"], "status": "ok"}
                //{"message": "Please check the security code we sent you and try again.", "status": "fail", "error_type": "sms_code_validation_code_invalid"}


                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountTwoFactorResponse>(exception);
            }
        }

        public async Task<IResult<AccountSecuritySettingsResponse>> GetSecuritySettingsInfoAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/account_security_info/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountSecuritySettingsResponse)null);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountSecuritySettingsResponse>(json);
                //{"phone_number": "+989174314006", "country_code": 98, "national_number": 9174314006, "is_phone_confirmed": true, "is_two_factor_enabled": true, "backup_codes": ["6714 2850", "5346 8102", "0681 3295", "9340 8652", "7560 2893"], "status": "ok"}

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountSecuritySettingsResponse>(exception);
            }
        }



        public async Task<IResult<AccountConfirmEmailResponse>> SendConfirmEmailAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/send_confirm_email/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"send_source", "edit_profile"}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
  
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountConfirmEmailResponse)null);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountConfirmEmailResponse>(json);
                //{"phone_number": "+989174314006", "country_code": 98, "national_number": 9174314006, "is_phone_confirmed": true, "is_two_factor_enabled": true, "backup_codes": ["6714 2850", "5346 8102", "0681 3295", "9340 8652", "7560 2893"], "status": "ok"}

                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountConfirmEmailResponse>(exception);
            }
        }


        public async Task<IResult<AccountSendSmsResponse>> SendSmsCodeAsync(string phoneNumber)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/send_sms_code/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    { "device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    { "phone_number", phoneNumber},

                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountSendSmsResponse)null);
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<AccountSendSmsResponse>(json);
                //{"phone_number_valid": true, "phone_verification_settings": {"max_sms_count": 2, "resend_sms_delay_sec": 60, "robocall_after_max_sms": true, "robocall_count_down_time_sec": 30}, "status": "ok"}


                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountSendSmsResponse>(exception);
            }
        }


        public async Task<IResult<AccountVerifySmsResponse>> VerifySmsCodeAsync(string phoneNumber, string verificationCode)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/verify_sms_code/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    { "device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    { "phone_number", phoneNumber},
                    { "verification_code", verificationCode}

                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountVerifySmsResponse)null);
                var obj = JsonConvert.DeserializeObject<AccountVerifySmsResponse>(json);
                //{"verified": false, "errors": {"verification_code": ["Please check the verification code we sent you and try again"]}, "status": "ok", "error_type": "sms_code_account_not_verified"}
                //{"verified": true, "phone_number": "+989174314006", "status": "ok"}
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<AccountVerifySmsResponse>(exception);
            }
        }


        //NOT COMPLETE
        public async Task<IResult<object>> EnablePresenceAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/set_presence_disabled/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"disabled", "0"},
                    { "_csrftoken", _user.CsrfToken}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(response.StatusCode);

                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountBesties)null);

                return null;
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<object>(exception);
            }
        }

        //NOT COMPLETE
        public async Task<IResult<object>> DisablePresenceAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/set_presence_disabled/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"disabled", "1"},
                    { "_csrftoken", _user.CsrfToken}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(response.StatusCode);
                Debug.WriteLine(json);
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.Fail("Status code: " + response.StatusCode, (AccountBesties)null);


                return null;
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<object>(exception);
            }
        }

        //NOT COMPLETE
        public async Task<IResult<object>> GetCommentFilterAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"accounts/get_comment_filter/");
                Debug.WriteLine(instaUri.ToString());

             
                var request = HttpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(response.StatusCode);
                Debug.WriteLine(json);
                //if (response.StatusCode != HttpStatusCode.OK)
                //    return Result.Fail("Status code: " + response.StatusCode, false);
                //{"config_value": 0, "status": "ok"}
                return null;
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<object>(exception);
            }
        }

    }
}
