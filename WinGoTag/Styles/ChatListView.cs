using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Styles
{
    public class ChatListView : ListView
    {
        private uint itemsSeen;
        private double averageContainerHeight;
        private bool loadingInProcess = false;

        public ChatListView()
        {
            // We'll manually trigger the loading of data incrementally and buffer for 2 pages worth of data
            this.IncrementalLoadingTrigger = IncrementalLoadingTrigger.None;

            // Since we'll have variable sized items we compute a running average of height to help estimate
            // how much data to request for incremental loading
            this.ContainerContentChanging += this.UpdateRunningAverageContainerHeight;
        }

        protected override void OnApplyTemplate()
        {
            var scrollViewer = this.GetTemplateChild("ScrollViewer") as ScrollViewer;

            if (scrollViewer != null)
            {
                scrollViewer.ViewChanged += (s, a) =>
                {
                    // Check if we should load more data when the scroll position changes.
                    // We only get this once the content/panel is large enough to be scrollable.
                    this.ProcessDataVirtualizationScrollOffsetsAsync(this.ActualHeight);
                };
            }

            base.OnApplyTemplate();
        }

        // We use ArrangeOverride to trigger incrementally loading data (if needed) when the panel is too small to be scrollable.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Allow the panel to arrange first
            var result = base.ArrangeOverride(finalSize);

            ProcessDataVirtualizationScrollOffsetsAsync(finalSize.Height);

            return result;
        }

        private async void ProcessDataVirtualizationScrollOffsetsAsync(double actualHeight)
        {
            var panel = this.ItemsPanelRoot as ItemsStackPanel;
            
            if (panel != null && !this.loadingInProcess)
            {
                if ((panel.FirstVisibleIndex != -1 && panel.FirstVisibleIndex * this.averageContainerHeight < actualHeight * this.IncrementalLoadingThreshold) ||
                    (panel.FirstVisibleIndex == -1 && Items.Count <= 1))
                {
                    var virtualizingDataSource = this.ItemsSource as ISupportIncrementalLoading;
                    if (virtualizingDataSource != null)
                    {
                        if (virtualizingDataSource.HasMoreItems)
                        {
                            double avgItemsPerPage = actualHeight / this.averageContainerHeight;
                            var itemsToLoad = (uint)(this.DataFetchSize * avgItemsPerPage);
                            if (itemsToLoad <= 0)
                            {
                                // We know there's data to be loaded so load at least one item
                                itemsToLoad = 1;
                            }

                            this.loadingInProcess = true;
                            
                            await virtualizingDataSource.LoadMoreItemsAsync(itemsToLoad);
                            this.loadingInProcess = false;
                        }
                    }
                }
            }
        }

        private void UpdateRunningAverageContainerHeight(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.ItemContainer != null && args.InRecycleQueue != true)
            {
                if (args.Phase == 0)
                {
                    // use the size of the very first placeholder as a starting point until
                    // we've seen the first item
                    if (this.averageContainerHeight == 0)
                    {
                        this.averageContainerHeight = args.ItemContainer.DesiredSize.Height;
                    }

                    args.RegisterUpdateCallback(1, this.UpdateRunningAverageContainerHeight);
                    args.Handled = true;
                }
                else if (args.Phase == 1)
                {
                    // set the content
                    args.ItemContainer.Content = args.Item;
                    args.RegisterUpdateCallback(2, this.UpdateRunningAverageContainerHeight);
                    args.Handled = true;
                }
                else if (args.Phase == 2)
                {
                    // refine the estimate based on the item's DesiredSize
                    this.averageContainerHeight = (this.averageContainerHeight * itemsSeen + args.ItemContainer.DesiredSize.Height) / ++itemsSeen;
                    args.Handled = true;
                }
            }

            
        }
    }

}
