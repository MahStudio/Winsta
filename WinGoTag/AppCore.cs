using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Core;

namespace WinGoTag
{
    public class AppCore
    {
        static Stack<DispatchedHandler> NavigationManager = new Stack<DispatchedHandler>();
        public static void ModerateBack(string emptyString)
        {
            try
            {
                NavigationManager.Pop();
            }
            catch
            {
            }
        }

        public static void ModerateBack()
        {
            try
            {
                var f = NavigationManager.Pop();
                f.Invoke();
            }
            catch
            {
                App.Current.Exit();
            }
        }

        public static void ModerateBack(DispatchedHandler Callback) => NavigationManager.Push(Callback);


        public static IInstaApi InstaApi
        {
            get; set;
        }

        public static Task<bool> RunAsync() => IsUserSessionStored();

        static async Task<bool> IsUserSessionStored()
        {
            try
            {
                if (InstaApi != null)
                {
                    InstaApi = null;
                    return false;
                }

                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserSession.dat", CreationCollisionOption.OpenIfExists);
                var stream = (await file.OpenAsync(FileAccessMode.Read)).AsStream();

                var User = "username"; var Pass = "password";
                // LoadUserInfo(out User, out Pass);
                // if (User == null || Pass == null) return false;
                InstaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(new UserSessionData { UserName = User, Password = Pass })
                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
                    .Build();
                InstaApi.LoadStateDataFromStream(stream);
                if (!InstaApi.IsUserAuthenticated)
                    await InstaApi.LoginAsync();

                return true;
            }
            catch (Exception /*ex*/)
            {
                InstaApi = null;
                return false;
            }
        }

        public static void SaveUserInfo(string Username, string Password, bool TwoFactorAccepted = true)
        {
            if ((Username == null || Password == null) && TwoFactorAccepted == true)
            {
                ApplicationData.Current.LocalSettings.Values["TwoFactorAccepted"] = true;
                return;
            }

            ApplicationData.Current.LocalSettings.Values["Username"] = Username;
            ApplicationData.Current.LocalSettings.Values["Password"] = Password;
            ApplicationData.Current.LocalSettings.Values["TwoFactorAccepted"] = TwoFactorAccepted;
        }

        public static void LoadUserInfo(out string Username, out string Password)
        {
            try
            {
                if (Convert.ToBoolean(ApplicationData.Current.LocalSettings.Values["TwoFactorAccepted"]))
                {
                    Username = ApplicationData.Current.LocalSettings.Values["Username"].ToString();
                    Password = ApplicationData.Current.LocalSettings.Values["Password"].ToString();
                }
                else { Username = null; Password = null; }
            }
            catch
            {
                Username = null; Password = null;
            }
        }

        public static async void RegisterNotifyTask()
        {
            try
            {
                var myTaskName = "NotifyTask";
                var myTaskEntryPoint = "InstaNotifications.NotifyTask";

                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                    if (cur.Value.Name == myTaskName)
                    {
                        ExtensionHelper.Output($"{myTaskEntryPoint} already registered");
                        return;
                    }

                await BackgroundExecutionManager.RequestAccessAsync();

                var taskBuilder =
                    new BackgroundTaskBuilder { Name = myTaskName, TaskEntryPoint = myTaskEntryPoint };

                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                var myFirstTask = taskBuilder.Register();
            }
            catch
            {
            }
        }

        public static void UnregisterNotifyTask()
        {
            try
            {
                var myTaskName = "NotifyTask";
                var myTaskEntryPoint = "InstaNotifications.NotifyTask";

                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                    if (cur.Value.Name == myTaskName)
                    {
                        cur.Value.Unregister(true);
                        ExtensionHelper.Output($"{myTaskEntryPoint} unregistered");
                        return;
                    }
            }
            catch
            {
            }
        }
    }
}