using InstaSharper.Classes;
using InstaSharper.Classes.Android.DeviceInfo;
using InstaSharper.Helpers;
using InstaSharper.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstaSharper.API.Processors
{
    internal class DiscoverProcessor : IDiscoverProcessor
    {
        private readonly AndroidDevice _deviceInfo;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly IInstaLogger _logger;
        private readonly UserSessionData _user;

        public DiscoverProcessor(AndroidDevice deviceInfo, UserSessionData user,
            IHttpRequestProcessor httpRequestProcessor, IInstaLogger logger)
        {
            _deviceInfo = deviceInfo;
            _user = user;
            _httpRequestProcessor = httpRequestProcessor;
            _logger = logger;
        }
        
        public async Task<IResult<DiscoverRecentSearchsResponse>> GetRecentSearchsAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + "fbsearch/recent_searches/");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DiscoverRecentSearchsResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DiscoverRecentSearchsResponse>(exception);
            }
        }

        public async Task<IResult<DicoverDefaultResponse>> ClearRecentSearchsAsync()
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + "fbsearch/clear_search_history/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DicoverDefaultResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DicoverDefaultResponse>(exception);
            }
        }

        public async Task<IResult<DiscoverSuggestionResponse>> GetSuggestedSearchesAsync(DiscoverSearchType searchType)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"fbsearch/suggested_searches/?type={searchType.ToString().ToLower()}");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DiscoverSuggestionResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DiscoverSuggestionResponse>(exception);
            }
        }
        // KAMEL NIST
        public async Task<IResult<DicoverDefaultResponse>> DiscoverPeopleAsync()
        {
            try
            {
                // KAMEL NIST
                
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + "discover/ayml/");
                Debug.WriteLine(instaUri.ToString());

                var data = new JObject
                {
                    { "phone_id", _deviceInfo.DeviceGuid.ToString()},
                    { "module","discover_people"},
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    { "paginate","true"}
                    //{"_uid", _user.LoggedInUder.Pk.ToString()},
                };
             
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DicoverDefaultResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DicoverDefaultResponse>(exception);
            }
        }

        public async Task<IResult<DiscoverSearchResponse>> SearchPeopleAsync(string text, int count = 30)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"users/search/?timezone_offset={TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalSeconds}&q={text}&count={count}");
                Debug.WriteLine(instaUri.ToString());

                var request = HttpHelper.GetDefaultRequest(HttpMethod.Get, instaUri, _deviceInfo);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DiscoverSearchResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DiscoverSearchResponse>(exception);
            }
        }

        public async Task<IResult<DiscoverRecentSearchsResponse>> AcceptFriendshipAsync(string userId)
        {
            try
            {

                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"friendships/approve/{userId}");
                Debug.WriteLine(instaUri.ToString());
                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"user_id", userId},
                    {"radio_type", "wifi"}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DiscoverRecentSearchsResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DiscoverRecentSearchsResponse>(exception);
            }
        }

        public async Task<IResult<DiscoverRecentSearchsResponse>> RejectFriendshipAsync(string userId)
        {
            try
            {
                var instaUri = new Uri(InstaApiConstants.BASE_INSTAGRAM_API_URL + $"friendships/ignore/{userId}");
                Debug.WriteLine(instaUri.ToString());
                var data = new JObject
                {
                    { "_csrftoken", _user.CsrfToken},
                    {"_uuid", _deviceInfo.DeviceGuid.ToString()},
                    {"_uid", _user.LoggedInUder.Pk.ToString()},
                    {"user_id", userId},
                    {"radio_type", "wifi"}
                };
                var request = HttpHelper.GetSignedRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                request.Headers.Host = "i.instagram.com";
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(json);
                var obj = JsonConvert.DeserializeObject<DiscoverRecentSearchsResponse>(json);
                return Result.Success(obj);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<DiscoverRecentSearchsResponse>(exception);
            }
        }
        
    }
}
