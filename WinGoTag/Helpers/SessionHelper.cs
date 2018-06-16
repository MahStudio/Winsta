using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinGoTag.Helpers
{
    public static class SessionHelper
    {
        public const string SessionName = "UserSession.dat";
        public const string BackupFolder = "BackupSessions";

        public static async Task BackupCurrentSession()
        {
            try
            {
                if (AppCore.InstaApi == null)
                    return;
                if (!AppCore.InstaApi.IsUserAuthenticated)
                    return;
                var username = AppCore.InstaApi.GetLoggedUser()?.UserName;
                if (string.IsNullOrEmpty(username))
                    return;
                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(BackupFolder, CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync($"{username}.dat", CreationCollisionOption.ReplaceExisting);
                var state = AppCore.InstaApi.GetStateDataAsStream();
                await FileIO.WriteTextAsync(file, state);
            }
            catch (Exception ex) { ex.ExceptionMessage("SessionHelper.BackupCurrentSession"); }
        }

        public static async Task<string> GetBackupSession(string username)
        {
            try
            {
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(BackupFolder);
                var file = await folder.GetFileAsync($"{username}.dat");
                var text = await FileIO.ReadTextAsync(file);
                return text;
            }
            catch (Exception ex) { ex.ExceptionMessage("SessionHelper.GetBackupSession"); }
            return string.Empty;
        }

        public static async Task<bool> IsBackupSessionAvailable(string username)
        {
            try
            {
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(BackupFolder);
                var file = await folder.GetFileAsync($"{username}.dat");
                var text = await FileIO.ReadTextAsync(file);
                if (string.IsNullOrEmpty(text))
                    return false;
                if (text.Length < 100)
                    return false;
                return true;
            }
            catch (Exception ex) { ex.ExceptionMessage("SessionHelper.GetBackupSession"); }
            return false;
        }

        public static async Task DeleteBackupSession(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return;
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(BackupFolder);
                var file = await folder.GetFileAsync($"{username}.dat");
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (Exception ex) { ex.ExceptionMessage("SessionHelper.DeleteBackupSession"); }
        }

        public static async Task DeleteCurrentSession()
        {
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(SessionName);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (Exception ex) { ex.ExceptionMessage("SessionHelper.DeleteCurrentSession"); }
        }
    }
}