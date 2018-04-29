using System;
using Windows.UI.Xaml;


class MultilingualHelpToolkit
{
    //GetString("LanguageOptionsSubTitle","Text")l
    public static string GetString(string Title, string Property)
    {
        Windows.ApplicationModel.Resources.ResourceLoader loader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
        var expected = loader.GetString(Title + "/" + Property);
        if (expected.Contains(@"\n"))
            expected = expected.Replace(@"\n", Environment.NewLine);
        return expected;
    }

    public FlowDirection GetObjectFlowDirection(string Title)
    {
        Windows.ApplicationModel.Resources.ResourceLoader loader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
        var expected = loader.GetString(Title + "/FlowDirection");
        if (expected.StartsWith("R") || expected.StartsWith("r"))
            return FlowDirection.RightToLeft;
        else return FlowDirection.LeftToRight;
    }
}

