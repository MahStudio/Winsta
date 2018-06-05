using InstaSharper.Classes.ResponseWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaSharper.Classes.Models
{
    public class InstaInlineCommentList : IInstaBaseList
    {
        public int ChildCommentCount { get; set; }

        public bool HasMoreTailChildComments { get; set; }

        public bool HasMoreHeadChildComments { get; set; }

        public string NextId { get; set; }

        public int NumTailChildComments { get; set; }

        public InstaComment ParentComment { get; set; }

        public List<InstaComment> ChildComments { get; set; }
    }
}
