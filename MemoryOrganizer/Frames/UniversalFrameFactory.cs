using System;
using System.Collections.Generic;

namespace MemoryOrganizer.Frames
{
    public class UniversalFrameFactory: IFrameListFactory
    {
        public List<IFrame> CreateList(int amount)
        {
            var frames = new List<IFrame>();
            for (var i = 0; i < amount; i++)
                frames.Add(new UniversalFrame(Guid.NewGuid()));
            return frames;
        }
    }
}