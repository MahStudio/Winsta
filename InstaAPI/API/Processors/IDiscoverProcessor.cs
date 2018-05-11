using InstaSharper.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaSharper.API.Processors
{
    public enum DiscoverSearchType
    {
        //'blended', 'users', 'hashtags', 'places'
        Blended,
        Users,
        Hashtags,
        Places
    }
    public interface IDiscoverProcessor
    {
        Task<IResult<DiscoverRecentSearchsResponse>> GetRecentSearchsAsync();

        Task<IResult<DicoverDefaultResponse>> ClearRecentSearchsAsync();

        Task<IResult<DiscoverSuggestionResponse>> GetSuggestedSearchesAsync(DiscoverSearchType searchType);


        Task<IResult<DicoverDefaultResponse>> DiscoverPeopleAsync();

        Task<IResult<DiscoverSearchResponse>> SearchPeopleAsync(string content, int count = 30);

        Task<IResult<DiscoverRecentSearchsResponse>> AcceptFriendshipAsync(string userId);

        Task<IResult<DiscoverRecentSearchsResponse>> RejectFriendshipAsync(string userId);

    }
}
