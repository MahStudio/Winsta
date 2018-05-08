using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinGoTag.DataBinding
{
    public class GenerateHomePage<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateHomePage(uint maxCount, Func<int, T> generator)
        {
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
            var res = await AppCore.InstaApi.GetUserTimelineFeedAsync(pagination);
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
            IEnumerable<InstaDirectInboxThread> tres = null;//
            var res = await AppCore.InstaApi.GetDirectInboxAsync();
            tres = res.Value.Inbox.Threads;
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

    public class GenerateExplorePage<T> : IncrementalLoadingBase
    {
        private int _LastPage = 1;
        PaginationParameters pagination;
        public GenerateExplorePage(uint maxCount, Func<int, T> generator)
        {
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
            _generator = generator;
            _maxCount = maxCount;
            _username = username;
            pagination = PaginationParameters.MaxPagesToLoad(1);
        }

        protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        {
            uint toGenerate = System.Math.Min(count, _maxCount - _count);
            // Wait for work 
            await Task.Delay(10);
            //http://getsongg.com/dapp/getnewcases?lang=en&tested
            IEnumerable<InstaUserShort> tres = null;//
            var res = await AppCore.InstaApi.GetUserFollowersAsync(_username, pagination);
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
