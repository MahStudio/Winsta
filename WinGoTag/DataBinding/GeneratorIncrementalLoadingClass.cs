using InstaAPI.Classes.Models;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinGoTag.DataBinding
{
    public class GenerateHomePage<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateHomePage(uint maxCount, Func<int, T> generator)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<InstaMedia>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaMedia> tres = null;//
            var res = await AppCore.InstaApi.GetUserTimelineFeedAsync(pagination);
            if (res.Value.NextId == null) HasMoreItems = false;
            pagination.NextId = res.Value.NextId;
            tres = res.Value.Medias;
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    public class GenerateDirectsList<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateDirectsList(uint maxCount, Func<int, T> generator)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if(!HasMoreItems)
                return (new List<InstaDirectInboxThread>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaDirectInboxThread> tres = null;//
            var res = await AppCore.InstaApi.GetDirectInboxAsync(pagination);
            if (res.Value.Inbox.OldestCursor == null) HasMoreItems = false;
            pagination.NextId = res.Value.Inbox.OldestCursor;
            tres = res.Value.Inbox.Threads;
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaDirectInboxThread>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    public class GenerateDirectThreadList<T> : ChatIncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        string ThreadID = "";
        public GenerateDirectThreadList(uint maxCount, Func<int, T> generator, string _thid)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
            ThreadID = _thid;
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if(!HasMoreItems)
                return (new List<InstaDirectInboxItem>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaDirectInboxItem> tres = null;//
            var res = await AppCore.InstaApi.GetDirectInboxThreadAsync(ThreadID, pagination);
            if (res.Value.OldestCursor == null) HasMoreItems = false;
            pagination.NextId = res.Value.OldestCursor;

            
            tres = res.Value.Items;
          
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaDirectInboxItem>()).ToArray();
            }
           
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    public class GenerateExplorePage<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateExplorePage(uint maxCount, Func<int, T> generator)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaMedia> tres = null;//
            var res = await AppCore.InstaApi.GetExploreFeedAsync(pagination);
            pagination.NextId = res.Value.NextId;
            tres = res.Value.Medias;
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    public class GenerateUserFollowers<T> : IncrementalLoadingBase
    {
        private string _username;
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateUserFollowers(uint maxCount, Func<int, T> generator, string username)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            _username = username;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<InstaMedia>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaUserShort> tres = null;//
            var res = await AppCore.InstaApi.GetUserFollowersAsync(_username, pagination);
            if (res.Value.NextId == null) HasMoreItems = false;
            pagination.NextId = res.Value.NextId;
            tres = res.Value.ToList();
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    public class GenerateUserFollowings<T> : IncrementalLoadingBase
    {
        private string _username;
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateUserFollowings(uint maxCount, Func<int, T> generator, string username)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            _username = username;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<InstaMedia>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaUserShort> tres = null;//
            var res = await AppCore.InstaApi.GetUserFollowingAsync(_username, pagination);
            if (res.Value.NextId == null) HasMoreItems = false;
            pagination.NextId = res.Value.NextId;
            tres = res.Value.ToList();
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    //var FollowingRecentActivity = await AppCore.InstaApi.GetFollowingRecentActivityAsync(PaginationParameters.MaxPagesToLoad(1));


    public class FollowingRecentActivity<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public FollowingRecentActivity(uint maxCount, Func<int, T> generator)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaRecentActivityFeed> tres = null;//
            var res = await AppCore.InstaApi.GetFollowingRecentActivityAsync(pagination);
            pagination.NextId = res.Value.NextId;
            tres = res.Value.Items;
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaRecentActivityFeed>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    //var RecentActivity = await AppCore.InstaApi.GetRecentActivityAsync(PaginationParameters.MaxPagesToLoad(1));

    public class RecentActivity<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public RecentActivity(uint maxCount, Func<int, T> generator)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaRecentActivityFeed> tres = null;//
            var res = await AppCore.InstaApi.GetRecentActivityAsync(pagination);
            if (res.Value.NextId == null) HasMoreItems = false;
            pagination.NextId = res.Value.NextId;
            tres = res.Value.Items;
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaRecentActivityFeed>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    public class GenerateComments<T> : IncrementalLoadingBase
    {
        private string _mediaid;
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateComments(uint maxCount, Func<int, T> generator, string mediaid)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            _mediaid = mediaid;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<InstaMedia>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);

            IEnumerable<InstaComment> tres = null;//
            var res = await AppCore.InstaApi.GetMediaCommentsAsync(_mediaid, pagination);
            pagination.NextId = res.Value.NextId;
            tres = res.Value.Comments.ToList();
            // inline comment support
            //if (tres != null)
            //    foreach (var item in tres)
            //    {
            //        // child comment
            //        if (item.ChildCommentCount > 0)
            //        {
            //            var p = PaginationParameters.MaxPagesToLoad(1);
            //            var inlines = await AppCore.InstaApi.GetMediaInlineCommentsAsync(_mediaid, item.Pk.ToString(),
            //                PaginationParameters.Empty);
            //            p.NextId = inlines.Value.NextId;
            //            // age edame dasht :
            //            if(inlines.Value.HasMoreTailChildComments)
            //            {
            //                var inlines2 = await AppCore.InstaApi
            //                    .GetMediaInlineCommentsAsync(_mediaid, item.Pk.ToString(), p);
            //            }
            //        }
            //    }

            HasMoreItems = res.Value.MoreComentsAvailable;
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }

    public class GenerateUserMedia<T> : IncrementalLoadingBase
    {
        private string _username;
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateUserMedia(uint maxCount, Func<int, T> generator, string username)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxpage = maxCount;
            _username = username;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<InstaMedia>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxpage - _count);
            // Wait for work 
            await Task.Delay(10);

            IEnumerable<InstaMedia> tres = null;//
            var res = await AppCore.InstaApi.GetUserMediaAsync(_username, pagination);
            if (res.Value == null) { HasMoreItems = false; return new List<InstaMedia>().ToArray(); }
            if (res.Value.NextId == null) HasMoreItems = false;
            pagination.NextId = res.Value.NextId;
            tres = res.Value.ToList();
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _LastPage < _maxpage;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxpage;
        #endregion 
    }

    public class GenerateUserTags<T> : IncrementalLoadingBase
    {
        private string _username;
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateUserTags(uint maxCount, Func<int, T> generator, string username)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            _username = username;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<InstaMedia>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);

            IEnumerable<InstaMedia> tres = null;//
            var res = await AppCore.InstaApi.GetUserTagsAsync(_username, pagination);
            pagination.NextId = res.Value.NextId;
            tres = res.Value.ToList();
            if (_LastPage == res.Value.Pages) HasMoreItems = false;
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }


    public class PictureLibarys<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public PictureLibarys(uint maxCount, Func<int, T> generator)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<StorageFile> tres = null;//
            var res = await KnownFolders.SavedPictures.GetFilesAsync();
            //var res = await AppCore.InstaApi.GetRecentActivityAsync(pagination);
            //pagination.NextId = res[0].Path;
            tres = res;
            // This code simply generates
            if (tres == null)
            {
                return (new List<string>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }


    public class GenerateStoryMediaViewers<T> : IncrementalLoadingBase
    {
        string StoryID;
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateStoryMediaViewers(uint maxCount, Func<int, T> generator, string storyid)
        {
            StoryID = storyid;
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<User1>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<User1> tres = null;//
            var res = await AppCore.InstaApi.StoryProcessor.GetStoryMediaViewers(StoryID, pagination);
            if(res.Value == null)
            {
                throw new Exception(res.Info.Message);
            }
            if (res.Value.NextMaxId == null) HasMoreItems = false;
            pagination.NextId = res.Value.NextMaxId;
            tres = res.Value.Users;
            // This code simply generates
            if (tres == null)
            {
                return (new List<User1>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }
    public class GenerateLikedFeeds<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateLikedFeeds(uint maxCount, Func<int, T> generator)
        {
            HasMoreItems = true;
            _generator = generator;
            _maxCount = maxCount;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            if (!HasMoreItems)
                return (new List<InstaMedia>()).ToArray();
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaMedia> tres = null;//
            var res = await AppCore.InstaApi.GetLikeFeedAsync(pagination);
            if (res.Value == null)
            {
                throw new Exception(res.Info.Message);
            }
            if (res.Value.NextId == null) HasMoreItems = false;
            pagination.NextId = res.Value.NextId;
            tres = res.Value.ToList();
            // This code simply generates
            if (tres == null)
            {
                return (new List<InstaMedia>()).ToArray();
            }
            var values = from j in Enumerable.Range((int)_count, (int)toGenerate)
                         select (object)_generator(j);
            _count += Convert.ToUInt32(tres.Count());
            _LastPage++;
            //App._MainPageInt++;
            return tres.ToArray();
        }

        protected override bool HasMoreItemsOverride()
        {
            return _count < _maxCount;
        }

        #region State

        Func<int, T> _generator;
        uint _count = 0;
        uint _maxCount;

        #endregion 
    }
}
