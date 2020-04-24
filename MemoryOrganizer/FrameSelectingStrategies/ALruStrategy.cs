using System;
using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class ALruStrategy: IOrganizingStrategy
    {
        public readonly List<IFrame> _queue = new List<IFrame>();

        public ALruStrategy(IPhysicalMemory physicalMemory)
        {
            foreach (var frame in physicalMemory)
            {
                if (!(frame is IFrameWithMarker)) throw new Exception("bad frame type");
                _queue.Add(frame);
            }
        }

        private void MoveFrameToEndOfQueue(IFrame frame)
        {
            _queue.Remove(frame);
            _queue.Add(frame);
        }

        public string Name => "Approximated LRU";
        public int PagesErrors { get; private set; } = 0;

        public IReplacingVisitor CreateReplacingVisitor(IPageReference pageReference)
        {
            PagesErrors++;

            Guid pageId = pageReference.PageId;
            Guid frameId = GetNextFrame().Id;
            return new ReplacingVisitor(pageId, frameId);
        }

        private IFrame GetNextFrame()
        {

            if (_queue.Any(f => f.Page == null))
            {
                return _queue.First(frame => frame.Page == null);
            }
            while (true)
            {
                var frame = (IFrameWithMarker)_queue[0];
                MoveFrameToEndOfQueue(frame);
                if (!frame.Bit) return frame;
                frame.Bit = false;
            }
        }
    }
}