using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace WinGoTag.View.SearchView
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        internal static GridViewItem itemList;
        public SearchPage()
        {
            this.InitializeComponent();
        }

        
        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void StoriesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = ((InstaStory)e.ClickedItem);
            //var item = ((InstaReelFeed)e.ClickedItem);
            GridViewItem itemAnimation = (GridViewItem)StoriesList.ContainerFromItem(item);
            itemList = itemAnimation;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image", itemAnimation);
            StoryFr.Navigate(typeof(StoryView.StoryViews), item);
        }

        private void StoryFr_Navigated(object sender, NavigationEventArgs e)
        {
            if (StoryFr.Content is StoryView.StoryViews)
            {
                return;
            }

            ConnectedAnimation imageAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("imageReturn");
            if (imageAnimation != null)
            { imageAnimation.TryStart(itemList); }
        }
    }
}
