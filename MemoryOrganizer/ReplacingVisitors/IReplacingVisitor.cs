using System;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.ReplacingVisitors
{
    public interface IReplacingVisitor
    {
        Guid PageId { get; }
        Guid FrameId { get; }
        IPage Page { get; set; }
    }
}
