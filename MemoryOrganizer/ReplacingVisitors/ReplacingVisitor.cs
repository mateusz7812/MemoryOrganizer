using System;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.ReplacingVisitors
{
    public class ReplacingVisitor: IReplacingVisitor
    {
        public ReplacingVisitor(Guid pageId, Guid frameId)
        {
            PageId = pageId;
            FrameId = frameId;
        }

        public Guid PageId { get; }
        public Guid FrameId { get; }
        public IPage Page { get; set; }
    }
}