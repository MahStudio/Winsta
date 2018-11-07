using InstagramApiSharp.Classes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WinGoTag.UserControls
{
    public sealed partial class InstaStoryFeedUC : UserControl, INotifyPropertyChanged
    {
        public InstaReelFeed Story
        {
            get
            {
                return (InstaReelFeed)GetValue(StoryProperty);
            }
            set
            {
                SetValue(StoryProperty, value);
                this.DataContext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Media"));
            }
        }
        public static readonly DependencyProperty StoryProperty = DependencyProperty.Register(
         "Story",
         typeof(InstaReelFeed),
         typeof(InstaStoryFeedUC),
         new PropertyMetadata(null)
        );

        public event PropertyChangedEventHandler PropertyChanged;

        public InstaStoryFeedUC()
        {
            this.InitializeComponent();
            this.DataContextChanged += InstaStoryFeedUC_DataContextChanged;
        }

        private void InstaStoryFeedUC_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            

            if (args.NewValue != null)
            {
                if (args.NewValue.GetType() == typeof(InstaStory))
                {
                    var value = DataContext as InstaStory;

                    if (value.LatestReelMedia == value.Seen.ToString()) { }
                    else { return; }
                    //Check Seen for unseen value
                    if (value.Seen.Year == 1970)
                    {

                    }
                    else { BorderStory.Stroke = ((SolidColorBrush)Application.Current.Resources["ApplicationSecondaryForegroundThemeBrush"]); BorderStory.StrokeThickness = 0.5; ColorInsta.Opacity = 0; }


                    if (value.User is null)
                    {
                        PictureUser.UriSource = new Uri(value.Owner.ProfilePicture);
                        Name.Text = value.Owner.ProfilePictureId;
                        //location:219106069:1526926991755
                        //string[] words = new string[256];
                        //words = value.Id.Split(':');
                        //Loation.Visibility = Visibility.Visible;
                        //var ForLocation = await AppCore.InstaApi.SearchLocation(Double.Parse(words[1]), Double.Parse(words[2]), words[0]);
                        //var ForLocation = await AppCore.InstaApi.SearchLocation(0, 0, value.Id);
                        //try { Name.Text = ForLocation.Value[0].Name; } catch { }
                    }
                    //var s = Story.Items.First();
                }
            }


            if (args.NewValue != null)
            {
                if (args.NewValue.GetType() == typeof(InstaReelFeed))
                {
                    var value = DataContext as InstaReelFeed;
                    
                    if(value.User.UserName == "You")
                    {
                        if (value.Items.Count == 0)
                        {
                            AddStory.Visibility = Visibility.Visible;
                            ColorInsta.Opacity = 0;
                        }
                    }

                    if (value.LatestReelMedia == value.Seen) { } else { return; }

                    if (value.Seen == 0)
                    {
                       
                    }
                    else { BorderStory.Stroke = ((SolidColorBrush)Application.Current.Resources["ApplicationSecondaryForegroundThemeBrush"]); BorderStory.StrokeThickness = 0.5; ColorInsta.Opacity = 0; }

                    //var s = Story.Items.First();
                }
            }
                

           
        }

        private void bit_ImageOpened(object sender, RoutedEventArgs e)
        {
            DoubleAnimation fade = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(fade, EllipsePicture);
            Storyboard.SetTargetProperty(fade, "Opacity");
            Storyboard openpane = new Storyboard();
            openpane.Children.Add(fade);
            openpane.Begin();
        }
    }
}
