//using InstagramApiSharp;
//using InstagramApiSharp.API;
//using InstagramApiSharp.API.Builder;
//using InstagramApiSharp.Classes;
//using InstagramApiSharp.Classes.Models;
//using InstagramApiSharp.Logger;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.ApplicationModel.Background;
//using Windows.Storage;

//namespace InstaNotifications
//{
//    public sealed class NotifyTask : IBackgroundTask
//    {
//        const string NotifyName = "notify.json";
//        BackgroundTaskDeferral deferral_;
//        IInstagramApiSharp InstagramApiSharp;
//        NotifyList Notifies;
//        public async void Run(IBackgroundTaskInstance taskInstance)
//        {
//            deferral_ = taskInstance.GetDeferral();
//            Debug.WriteLine("NotifyTask started");
//            try
//            {
//                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserSession.dat", CreationCollisionOption.OpenIfExists);
//                //var r = await FileIO.ReadTextAsync(file);
//                var stream = (await file.OpenAsync(FileAccessMode.Read)).AsStream();
                
//                string User = "username"; string Pass = "password";
//                InstagramApiSharp = InstagramApiSharpBuilder.CreateBuilder()
//                    .SetUser(new UserSessionData { UserName = User, Password = Pass })
//                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
//                    .Build();
//                InstagramApiSharp.LoadStateDataFromStream(stream);
//                if (!InstagramApiSharp.IsUserAuthenticated)
//                {
//                    await InstagramApiSharp.LoginAsync();
//                }
//                if (!InstagramApiSharp.IsUserAuthenticated)
//                    return;
//                var activities = await InstagramApiSharp.FeedProcessor.GetRecentActivityFeedAsync(PaginationParameters.MaxPagesToLoad(1));
//                Notifies = await Load();
//                if (Notifies == null)
//                    Notifies = new NotifyList();
//                if (activities.Succeeded)
//                {
//                    const int MAX = 9;
//                    int ix = 0;
//                    foreach (var item in activities.Value.Items)
//                    {
//                        var text = item.Text;
//                        if (item.Text.Contains(" "))
//                        {
//                            text = text.Substring(text.IndexOf(" ") +1);
//                            text = text.TrimStart();
//                        }
//                        if (!Notifies.IsExists(text))
//                        {
//                            try
//                            {
//                                var n = new NotifyClass
//                                {
//                                    IsShowing = false,
//                                    ProfileId = item.ProfileId,
//                                    ProfilePicture = item.ProfileImage,
//                                    Text = text,
//                                    TimeStamp = item.TimeStamp.ToString(),
//                                    Type = item.Type,
//                                };
                        
//                                if (item.InlineFollow == null)
//                                {
//                                    var user = await InstagramApiSharp.UserProcessor.GetUserInfoByIdAsync(item.ProfileId);
//                                    if (user.Succeeded)
//                                    {
//                                        n.Username = user.Value.Username;
//                                    }
//                                }
//                                else
//                                {
//                                    n.Username = item.InlineFollow.User.UserName;
//                                    n.IsFollowingYou = item.InlineFollow.IsFollowing;
//                                }
//                                Notifies.Add(n);
//                            }
//                            catch { }
//                        }
//                        ix++;
//                        if (ix > MAX)
//                            break;
//                    }
//                    var list = Notifies;
//                    list.Reverse();
//                    for (int i = 0; i < list.Count; i++)
//                    {
//                        var item = list[i];
//                        if (!string.IsNullOrEmpty(item.Username))
//                        {
//                            if (!item.IsShowing)
//                            {
//                                NotifyHelper.CreateNotifyAction($"@{item.Username}",
//                                    item.Text,
//                                    item.ProfilePicture);
//                                Notifies[i].IsShowing = true;

//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine("Notify ex: " + ex.Message);
//                Debug.WriteLine("Source: " + ex.Source);
//                Debug.WriteLine("StackTrace: " + ex.StackTrace);
//            }
//            await Save();
//            await Task.Delay(1000);
//            deferral_.Complete();
//        }

//        async Task Save()
//        {
//            try
//            {
//                if (Notifies == null)
//                    return;
//                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(NotifyName, CreationCollisionOption.ReplaceExisting);
//                var json = JsonConvert.SerializeObject(Notifies);
//                await Task.Delay(250);
//                await FileIO.WriteTextAsync(file, json);
//            }
//            catch { }
//        }
//        async Task<NotifyList> Load()
//        {
//            NotifyList list = null;
//            try
//            {
//                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(NotifyName, CreationCollisionOption.OpenIfExists);
//                var json = await FileIO.ReadTextAsync(file);
//                list = JsonConvert.DeserializeObject<NotifyList>(json);
//            }
//            catch { }
//            return list;
//        }
//    }

//    public class NotifyList : List<NotifyClass>
//    {
//        public bool IsExists(string text)
//        {
//            return this.Any(x => x.Text.ToLower().StartsWith(text.ToLower()));
//        }
//        public int GetIndex(string text)
//        {
//            return this.FindIndex(x => x.Text.ToLower().StartsWith(text.ToLower()));
//        }
//    }

//    public sealed class NotifyClass
//    {
//        public bool IsShowing { get; set; }
//        public string Text { get; set; }

//        public string Username { get; set; }
//        public long ProfileId { get; set; }
//        public string ProfilePicture { get; set; }
//        public bool IsFollowingYou { get; set; }
//        public string TimeStamp { get; set; }
//        public int Type { get; set; }
//        //public InstaRecentActivityFeed ActivityFeed { get; set; }

//    }
//}

