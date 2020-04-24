using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class LruStrategy: IOrganizingStrategy
    {
        private readonly IPhysicalMemory _physicalMemory;

        public LruStrategy(IPhysicalMemory physicalMemory)
        {
            if (physicalMemory.Any(f => !(f is IFrameWithTimeStamps)))
                throw new Exception("bad frame type");
            _physicalMemory = physicalMemory;
        }

        public string Name { get; } = "LRU";
        public int PagesErrors { get; private set; } = 0;

        public IReplacingVisitor CreateReplacingVisitor(IPageReference pageReference)
        {
            PagesErrors++;

            var pageId = pageReference.PageId;
            var frameId = GetLeastRecentlyUsedFrame().Id;
            return new ReplacingVisitor(pageId, frameId);
        }

        private IFrame GetLeastRecentlyUsedFrame()
        {
            var frame = _physicalMemory.OrderBy(f =>
            {
                var frameWithTimeStamps = (IFrameWithTimeStamps) f;
                var lastUsage = frameWithTimeStamps.LastUsage;
                return lastUsage;
            }).FirstOrDefault();
            return frame;
        }
    }
}
