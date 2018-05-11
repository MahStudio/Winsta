using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinGoTag.View.UserViews;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.SearchView
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {

        public SearchPage()
        {
            this.InitializeComponent();
            EditFr.Navigate(typeof(Page));
        }


        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var query = SearchBox.Text;

            switch (PivotSearch.SelectedIndex)
            {
                case 0:
                    //var t1 = await AppCore.InstaApi.DiscoverProcessor.DiscoverPeopleAsync();
                    //var t2 = await AppCore.InstaApi.DiscoverProcessor.GetRecentSearchsAsync();
                    //var t3 = await AppCore.InstaApi.DiscoverProcessor.GetSuggestedSearchesAsync(InstaSharper.API.Processors.DiscoverSearchType.Users);
                    break;

                case 1:
                    var People = await AppCore.InstaApi.DiscoverProcessor.SearchPeopleAsync(query);
                    PeopleList.ItemsSource = People.Value.Users;
                    break;

                case 2:
                    var ForTag = await AppCore.InstaApi.SearchHashtag(query);
                    TagsList.ItemsSource = ForTag.Value;
                    break;

                case 3:
                    var ForLocation = await AppCore.InstaApi.SearchLocation(0, 0, query);
                    PlacesList.ItemsSource = ForLocation.Value;
                    break;
            }
        }


        private void CancelBT_Click(object sender, RoutedEventArgs e)
        {
            AnimationGrid(GridSearch, 1, 0, Visibility.Collapsed);
            //GridSearch.Visibility = Visibility.Collapsed;
            CancelBT.Visibility = Visibility.Collapsed;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AnimationGrid(GridSearch, 0, 1, Visibility.Visible);
            //GridSearch.Visibility = Visibility.Visible;
            CancelBT.Visibility = Visibility.Visible;
        }

        private void AnimationGrid(Grid sender, double From, double To, Visibility visibility)
        {
            DoubleAnimation fade = new DoubleAnimation()
            {
                From = From,
                To = To,
                Duration = TimeSpan.FromSeconds(0.4),
                EnableDependentAnimation = true
            };
            Storyboard.SetTarget(fade, sender);
            Storyboard.SetTargetProperty(fade, "Opacity");
            Storyboard openpane = new Storyboard();
            openpane.Children.Add(fade);
            openpane.Begin();
            Task.Delay(04);
            GridSearch.Visibility = visibility;
        }



        private void ExploreFr_Loaded(object sender, RoutedEventArgs e)
        {
            ExploreFr.Navigate(typeof(ExploreView));
        }

        private void PeopleList_ItemClick(object sender, ItemClickEventArgs e)
        {
            EditFr.Navigate(typeof(UserProfileView), e.ClickedItem);
        }
    }
}
