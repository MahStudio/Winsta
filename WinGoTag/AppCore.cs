using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;

namespace WinGoTag
{
    public class AppCore
    {
        private static Stack<DispatchedHandler> _NavigationManager = new Stack<DispatchedHandler>();
        public static void ModerateBack(string EmptyString)
        {
            try
            {
                _NavigationManager.Pop();
            }
            catch
            {
            }
        }

        public static void ModerateBack()
        {
            try
            {
                var f = _NavigationManager.Pop();
                f.Invoke();
            }
            catch 
            {
                App.Current.Exit();
            }
        }

        public static void ModerateBack(DispatchedHandler Callback)
        {
            _NavigationManager.Push(Callback);
        }

        public static IInstaApi InstaApi
        {
            get; set;
        }

        public static async Task<bool> RunAsync()
        {
            return await IsUserSessionStored();
        }

        private static async Task<bool> IsUserSessionStored()
        {
            try
            {
                if (InstaApi != null)
                {
                    InstaApi = null;
                    return false;
                }
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserSession.dat", CreationCollisionOption.OpenIfExists);
                var r = await FileIO.ReadTextAsync(file);
                string User = "username"; string Pass="password";
                //LoadUserInfo(out User, out Pass);
                //if (User == null || Pass == null) return false;
                InstaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(new UserSessionData { UserName = User, Password = Pass })
                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
                    .Build();
                InstaApi.LoadStateDataFromStream(r);
                if(!InstaApi.IsUserAuthenticated)
                {
                    await InstaApi.LoginAsync();
                }
                return true;
            }
            catch (Exception ex)
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
    }
}
