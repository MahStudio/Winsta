using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinGoTag
{
    public class AppCore
    {
        public static IInstaApi InstaApi
        {
            get; set;
        }

        public static async Task RunAsync()
        {
            await IsUserSessionStored();
        }

        private static async Task<bool> IsUserSessionStored()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("UserSession.dat", CreationCollisionOption.OpenIfExists);
                var r = await FileIO.ReadTextAsync(file);
                UserSessionData User = new UserSessionData()
                {
                    UserName = "",
                    Password = ""
                };
                InstaApi = InstaApiBuilder.CreateBuilder()
                    .SetUser(User)
                    .UseLogger(new DebugLogger(LogLevel.Exceptions))
                    .Build();
                InstaApi.LoadStateDataFromStream(r);
                return true;
            }
            catch(Exception ex)
            {
                InstaApi = null;
                return false;
            }
        }
    }
}
