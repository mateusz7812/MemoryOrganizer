using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Pages;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.Memories
{
    public class PhysicalMemory: IPhysicalMemory
    {
        protected readonly List<IFrame> FrameList;

        public PhysicalMemory(List<IFrame> frameList)
        {
            FrameList = frameList;
        }

        public void Accept(IReplacingVisitor visitor) => 
            FrameList.First(f => f.Id.Equals(visitor.FrameId)).Page = visitor.Page;

        public bool Contains(Guid pageId) => 
            FrameList.Count(f => f.HavePage(pageId)) != 0;

        public IPhysicalMemory GetSubMemory(int amount)
        {
            var frames = FrameList.GetRange(0, amount);
            return new PhysicalMemory(frames);
        }

        public bool HaveFreeFrames { get; }


        public IEnumerator<IFrame> GetEnumerator() => FrameList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
