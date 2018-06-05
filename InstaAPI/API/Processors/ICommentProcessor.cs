using System.Threading.Tasks;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Classes.ResponseWrappers;

namespace InstaSharper.API.Processors
{
    public interface ICommentProcessor
    {
        Task<IResult<InstaCommentList>>
            GetMediaCommentsAsync(string mediaId, PaginationParameters paginationParameters);
        Task<IResult</*InstaInlineCommentList*/InstaInlineCommentListResponse>> 
            GetMediaInlineCommentsAsync(string mediaId, string targetCommentId, PaginationParameters paginationParameters);


        Task<IResult<InstaComment>> CommentMediaAsync(string mediaId, string text);
        Task<IResult<bool>> DeleteCommentAsync(string mediaId, string commentId);
        Task<IResult<InstaComment>> InlineCommentMediaAsync(string mediaId, string targetCommentId, string text);
    }
}