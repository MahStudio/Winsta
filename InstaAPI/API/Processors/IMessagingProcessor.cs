using System.Threading.Tasks;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;

namespace InstaSharper.API.Processors
{
    public interface IMessagingProcessor
    {
        Task<IResult<InstaDirectInboxContainer>> GetDirectInboxAsync(PaginationParameters paginationParameters, string Query = "");
        Task<IResult<InstaDirectInboxThread>> GetDirectInboxThreadAsync(string threadId, PaginationParameters paginationParameters);

        Task<IResult<InstaDirectInboxThreadList>> SendDirectMessage(string recipients, string threadIds,
            string text);

        Task<IResult<InstaRecipients>> GetRecentRecipientsAsync();
        Task<IResult<InstaRecipients>> GetRankedRecipientsAsync();
    }
}