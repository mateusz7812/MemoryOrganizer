using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class RandomStrategy: IOrganizingStrategy
    {
        private readonly List<IFrame> _frames;

        public RandomStrategy(IPhysicalMemory physicalMemory)
        {
            _frames = physicalMemory.ToList();
        }

        public string Name => "Random";
        public int PagesErrors { get; private set; } = 0;
        public IReplacingVisitor CreateReplacingVisitor(IPageReference pageReference)
        {
            PagesErrors++;

            var pageId = pageReference.PageId;
            var frameId = GetRandomFrame();
            return new ReplacingVisitor(pageId, frameId);
        }

        private Guid GetRandomFrame()
        {
            var provider = new RNGCryptoServiceProvider();
            var box = new byte[1];
            var framesCount = _frames.Count();
            do provider.GetBytes(box);
            while (!(box[0] < framesCount * (Byte.MaxValue / framesCount)));
            var index = (box[0] % framesCount);
            return _frames[index].Id;
        }
    }
}