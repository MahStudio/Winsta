using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace WinGoTag.Styles
{
    public class ChatListView : ListView
    {
        uint itemsSeen;
        double averageContainerHeight;
        bool loadingInProcess;

        public ChatListView()
        {
            // We'll manually trigger the loading of data incrementally and buffer for 2 pages worth of data
            IncrementalLoadingTrigger = IncrementalLoadingTrigger.None;

            // Since we'll have variable sized items we compute a running average of height to help estimate
            // how much data to request for incremental loading
            ContainerContentChanging += UpdateRunningAverageContainerHeight;
        }

        protected override void OnApplyTemplate()
        {
            var scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;

            if (scrollViewer != null)
            {
                scrollViewer.ViewChanged += (s, a) =>
                {
                    // Check if we should load more data when the scroll position changes.
                    // We only get this once the content/panel is large enough to be scrollable.
                    ProcessDataVirtualizationScrollOffsetsAsync(ActualHeight);
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

        async void ProcessDataVirtualizationScrollOffsetsAsync(double actualHeight)
        {
            var panel = ItemsPanelRoot as ItemsStackPanel;

            if (panel != null && !loadingInProcess)
            {
                if ((panel.FirstVisibleIndex != -1 && panel.FirstVisibleIndex * averageContainerHeight < actualHeight * IncrementalLoadingThreshold) ||
                    (panel.FirstVisibleIndex == -1 && Items.Count <= 1))
                {
                    var virtualizingDataSource = ItemsSource as ISupportIncrementalLoading;
                    if (virtualizingDataSource != null)
                    {
                        if (virtualizingDataSource.HasMoreItems)
                        {
                            var avgItemsPerPage = actualHeight / averageContainerHeight;
                            var itemsToLoad = (uint)(DataFetchSize * avgItemsPerPage);
                            if (itemsToLoad <= 0)
                                itemsToLoad = 1;


                            loadingInProcess = true;

                            await virtualizingDataSource.LoadMoreItemsAsync(itemsToLoad);
                            loadingInProcess = false;
                        }
                    }
                }
            }
        }

        void UpdateRunningAverageContainerHeight(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.ItemContainer != null && args.InRecycleQueue != true)
            {
                if (args.Phase == 0)
                {
                    // use the size of the very first placeholder as a starting point until
                    // we've seen the first item
                    if (averageContainerHeight == 0)
                        averageContainerHeight = args.ItemContainer.DesiredSize.Height;


                    args.RegisterUpdateCallback(1, UpdateRunningAverageContainerHeight);
                    args.Handled = true;
                }
                else if (args.Phase == 1)
                {
                    // set the content
                    args.ItemContainer.Content = args.Item;
                    args.RegisterUpdateCallback(2, UpdateRunningAverageContainerHeight);
                    args.Handled = true;
                }
                else if (args.Phase == 2)
                {
                    // refine the estimate based on the item's DesiredSize
                    averageContainerHeight = (averageContainerHeight * itemsSeen + args.ItemContainer.DesiredSize.Height) / ++itemsSeen;
                    args.Handled = true;
                }
            }
        }
    }
}