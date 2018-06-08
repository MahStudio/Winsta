using InstaAPI;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace InstaNotifications
{
    public sealed class NotifyTask : IBackgroundTask
    {
        const string NotifyName = "notify.json";
        BackgroundTaskDeferral deferral_;
        IInstaApi InstaApi;
        NotifyList Notifies;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral_ = taskInstance.GetDeferral();
            Debug.WriteLine("NotifyTask started");
            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserSession.dat", CreationCollisionOption.OpenIfExists);
                var r = await FileIO.ReadTextAsync(file);
                if (string.IsNullOrEmpty(r))
                    return;
                if (r.Length < 100)
                    return;
                string User = "username"; string Pass = "password";
                InstaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(new UserSessionData { UserName = User, Password = Pass })
                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
                    .Build();
                InstaApi.LoadStateDataFromStream(r);
                if (!InstaApi.IsUserAuthenticated)
                {
                    await InstaApi.LoginAsync();
                }
                if (!InstaApi.IsUserAuthenticated)
                    return;
                var activities = await InstaApi.GetRecentActivityAsync(PaginationParameters.MaxPagesToLoad(1));
                Notifies = await Load();
                if (Notifies == null)
                    Notifies = new NotifyList();
                if (activities.Succeeded)
                {
                    const int MAX = 9;
                    int ix = 0;
                    foreach (var item in activities.Value.Items)
                    {
                        if (!Notifies.IsExists(item.Text))
                        {
                            try
                            {
                                var n = new NotifyClass
                                {
                                    IsShowing = false,
                                    ProfileId = item.ProfileId,
                                    ProfilePicture = item.ProfileImage,
                                    Text = item.Text,
                                    TimeStamp = item.TimeStamp.ToString(),
                                    Type = item.Type,
                                };
                                if(item.Text.Contains(" "))
                                {
                                    var text = item.Text;
                                    text = text.Substring(text.IndexOf(" "));
                                    n.Text = text;
                                }
                                if (item.InlineFollow == null)
                                {
                                    var user = await InstaApi.GetUserInfoByIdAsync(item.ProfileId);
                                    if (user.Succeeded)
                                    {
                                        n.Username = user.Value.Username;
                                    }
                                }
                                else
                                {
                                    n.Username = item.InlineFollow.User.UserName;
                                    n.IsFollowingYou = item.InlineFollow.IsFollowing;
                                }
                                Notifies.Add(n);
                            }
                            catch { }
                        }
                        ix++;
                        if (ix > MAX)
                            break;
                    }
                    var list = Notifies;
                    list.Reverse();
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        if (!string.IsNullOrEmpty(item.Username))
                        {
                            if (!item.IsShowing)
                            {
                                NotifyHelper.CreateNotifyAction($"@{item.Username}",
                                    item.Text,
                                    item.ProfilePicture);
                                Notifies[i].IsShowing = true;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Notify ex: " + ex.Message);
                Debug.WriteLine("Source: " + ex.Source);
                Debug.WriteLine("StackTrace: " + ex.StackTrace);
            }
            await Save();
            await Task.Delay(1000);
            deferral_.Complete();
        }

        async Task Save()
        {
            try
            {
                if (Notifies == null)
                    return;
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(NotifyName, CreationCollisionOption.ReplaceExisting);
                var json = JsonConvert.SerializeObject(Notifies);
                await Task.Delay(250);
                await FileIO.WriteTextAsync(file, json);
            }
            catch { }
        }
        async Task<NotifyList> Load()
        {
            NotifyList list = null;
            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(NotifyName, CreationCollisionOption.OpenIfExists);
                var json = await FileIO.ReadTextAsync(file);
                list = JsonConvert.DeserializeObject<NotifyList>(json);
            }
            catch { }
            return list;
        }
    }
}
