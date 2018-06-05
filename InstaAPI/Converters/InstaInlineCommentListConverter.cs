using System;
using System.Collections.Generic;
using InstaSharper.Classes.Models;
using InstaSharper.Classes.ResponseWrappers;

namespace InstaSharper.Converters
{
    internal class InstaInlineCommentListConverter : IObjectConverter<InstaInlineCommentList, InstaInlineCommentListResponse>
    {
        public InstaInlineCommentListResponse SourceObject { get; set; }

        public InstaInlineCommentList Convert()
        {
            var commentList = new InstaInlineCommentList
            {
                ChildCommentCount = SourceObject.ChildCommentCount,
                HasMoreHeadChildComments = SourceObject.HasMoreHeadChildComments,
                HasMoreTailChildComments = SourceObject.HasMoreTailChildComments,
                NextId = SourceObject.NextId,
                NumTailChildComments = SourceObject.NumTailChildComments,
                ParentComment = ConvertersFabric.Instance.GetCommentConverter(SourceObject.ParentComment).Convert()
            };
            if (SourceObject.ChildComments == null || !(SourceObject?.ChildComments?.Count > 0)) return commentList;
            foreach (var commentResponse in SourceObject.ChildComments)
            {
                var converter = ConvertersFabric.Instance.GetCommentConverter(commentResponse);
                commentList.ChildComments.Add(converter.Convert());
            }
            return commentList;
        }
    }
}
