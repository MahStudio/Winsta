using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Core;

namespace WinGoTag
{
    public static class ExtensionHelper
    {
        public static async void ShowMessage(this string source, string title = "", CoreDispatcher dispatcher = null)
        {
            if (dispatcher == null)
                await new MessageDialog(source, title).ShowAsync();
            else
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await new MessageDialog(source, title).ShowAsync());
        }
        public static void ExceptionMessage(this Exception ex, string name = "")
        {
            Output($"{name} ex: {ex.Message}");
            Output($"Source: {ex.Source}");
            Output($"StackTrace: {ex.StackTrace}");
            Output();
        }
        public static void ShowInOutput(this object obj)
        {
            Output(obj);
        }
        public static void Output(object content = null)
        {
            if (content == null)
                Debug.WriteLine("");
            else
                Debug.WriteLine(Convert.ToString(content));
        }
        public static void Output(params string[] contents)
        {
            if (contents == null)
                Debug.WriteLine("");
            else
                Debug.WriteLine(string.Join("\n", contents));
        }

        public static async void OpenUrl(this string url)
        {
            var options = new Windows.System.LauncherOptions
            {
                TreatAsUntrusted = false
            };
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url), options);
        }
    }
}
