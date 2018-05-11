using InstaSharper.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InstaSharper.API.Processors
{
    public interface ILiveProcessor
    {

        Task<IResult<object>> SeenBroadcastAsync(string broadcastId, string pk);

        Task<IResult<BroadcastLiveHeartBeatViewerCountResponse>> GetHeartBeatAndViewerCountAsync(string broadcastId);

        Task<IResult<BroadcastFinalViewerListResponse>> GetFinalViewerListAsync(string broadcastId);


        //Task<IResult<object>> NotifyToFriendsAsync();

        Task<IResult<BroadcastSuggestedResponse>> GetSuggestedBroadcastsAsync();

        Task<IResult<DiscoverTopLiveResponse>> GetDiscoverTopLiveAsync();

        Task<IResult<BroadcastTopLiveStatusResponse>> GetTopLiveStatusAsync(params string[] broadcastIds);

        Task<IResult<BroadcastInfoResponse>> GetInfoAsync(string broadcastId);

        Task<IResult<BroadcastViewerListResponse>> GetViewerListAsync(string broadcastId);

        Task<IResult<BroadcastViewerListResponse>> GetPostLiveViewerListAsync(string broadcastId, int? maxId = null);

        Task<IResult<BroadcastSendCommentResponse>> CommentAsync(string broadcastId, string commentText);

        Task<IResult<BroadcastPinUnpinResponse>> PinCommentAsync(string broadcastId,string commentId);

        Task<IResult<BroadcastPinUnpinResponse>> UnPinCommentAsync(string broadcastId, string commentId);

        Task<IResult<BroadcastCommentResponse>> GetCommentsAsync(string broadcastId, int lastCommentTs = 0, int commentsRequested = 4);

        Task<IResult<object>> GetPostLiveCommentsAsync(string broadcastId, int startingOffset = 0, string encodingTag = "instagram_dash_remuxed");

        Task<IResult<BroadcastCommentEnableDisableResponse>> EnableCommentsAsync(string broadcastId);

        Task<IResult<BroadcastCommentEnableDisableResponse>> DisableCommentsAsync(string broadcastId);

        Task<IResult<BroadcastLikeResponse>> LikeAsync(string broadcastId,int likeCount = 1);

        Task<IResult<BroadcastLikeResponse>> GetLikeCountAsync(string broadcastId, int likeTs = 0);

        Task<IResult<object>> GetPostLiveLikesAsync(string broadcastId,int startingOffset = 0, string encodingTag = "instagram_dash_remuxed");

        Task<IResult<BroadcastAddToPostLiveResponse>> AddToPostLiveAsync(string broadcastId);
        Task<IResult<BroadcastDefaultResponse>> DeletePostLiveAsync(string broadcastId);



        // broadcast create, start, end

        Task<IResult<BroadcastCreateResponse>> CreateAsync(int previewWidth = 720, int previewHeight = 1184, string broadcastMessage = "");

        Task<IResult<BroadcastStartResponse>> StartAsync(string broadcastId, bool sendNotifications);

        Task<IResult<BroadcastDefaultResponse>> EndAsync(string broadcastId, bool endAfterCopyrightWarning = false);

    }
}
