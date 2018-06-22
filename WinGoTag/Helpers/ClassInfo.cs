using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;

class ClassInfo
{
    public enum DeviceTypeEnum
    {
        Phone = 1,
        Tablet = 2,
        XBOX = 3
    }

    public static DeviceTypeEnum DeviceType()
    {
        var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
        if (qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"].ToLower().Contains("xbox"))
        {
            return DeviceTypeEnum.XBOX;
        }
        return ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")
                ? DeviceTypeEnum.Phone : DeviceTypeEnum.Tablet;
    }
}

public static class ClassProInfo
{
    public static string SystemFamily { get; }
    public static string SystemVersion { get; }
    public static string SystemArchitecture { get; }
    public static string ApplicationName { get; }
    public static string ApplicationVersion { get; }
    public static string DeviceManufacturer { get; }
    public static string DeviceModel { get; }
    public static string DeviceID { get; }

    public static bool IsWindowsMobile()
    {
        if (SystemFamily == "Windows.Mobile")
            return true;
        else return false;
    }

    public static bool IsWindowsDesktop()
    {
        if (SystemFamily == "Windows.Desktop")
            return true;
        else return false;
    }

    static ClassProInfo()
    {
        // get the system family name
        var ai = AnalyticsInfo.VersionInfo;
        SystemFamily = ai.DeviceFamily;

        // get the system version number
        string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
        ulong v = ulong.Parse(sv);
        ulong v1 = (v & 0xFFFF000000000000L) >> 48;
        ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
        ulong v3 = (v & 0x00000000FFFF0000L) >> 16;
        ulong v4 = (v & 0x000000000000FFFFL);
        SystemVersion = $"{v1}.{v2}.{v3}.{v4}";

        // get the package architecure
        var package = Package.Current;
        SystemArchitecture = package.Id.Architecture.ToString();

        // get the user friendly app name
        ApplicationName = package.DisplayName;

        // get the app version
        PackageVersion pv = package.Id.Version;
        ApplicationVersion = $"{pv.Major}.{pv.Minor}.{pv.Build}.{pv.Revision}";

        // get the device manufacturer and model name
        var eas = new EasClientDeviceInformation();
        DeviceManufacturer = eas.SystemManufacturer;
        DeviceModel = eas.SystemProductName;
        DeviceID = eas.Id.ToString();
    }
}