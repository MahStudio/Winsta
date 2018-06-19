using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;


namespace InstaNotifications
{
    public sealed class NotifyHelper
    {
        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        ////////////////////////      NOTIFY CREATION     ////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////

        public static void CreateNotifyEmpty(string subject, string content)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = subject
                            },
                            new AdaptiveText()
                            {
                                Text = content
                            }
                        }
                    }
                }
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }


        public static void CreateNotifyLaunchAction(string subject, string content, string launchAction)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = subject
                            },
                            new AdaptiveText()
                            {
                                Text = content
                            }
                        }
                    }
                },
                Launch = "notifyAction=" + launchAction
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
        public static void CreateNotifyAction(string subject, string content, string img)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = subject
                            },
                            new AdaptiveText()
                            {
                                Text = content
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = img,
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                },
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public static void CreateNotifyButtonAction(string subject, string content, string img, string action)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveImage()
                            {
                                Source = img
                            },
                            new AdaptiveText()
                            {
                                Text = subject
                            },
                            new AdaptiveText()
                            {
                                Text = content
                            }
                        }
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Play", new QueryString()
                        {
                            { "action", "play" },
                            { "url", action }

                        }.ToString())
                        {
                            ActivationType = ToastActivationType.Foreground
                        },
                        new ToastButton("Download", new QueryString()
                        {
                            { "action", "download" },
                            { "url", action }

                        }.ToString())
                        {
                            ActivationType = ToastActivationType.Foreground
                        }
                    }
                },
                Launch = "actionFromNotify=" + action
            };

            // Create the toast notification
            var toastNotif = new ToastNotification(toastContent.GetXml());

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

    }
}
