using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemoryOrganizer.Frames
{
    class FrameListFactory : IFrameListFactory
    {
        public List<IFrame> CreateList(int amount)
        {
            var frames = new List<IFrame>();
            for (var i = 0; i < amount; i++)
                frames.Add(new Frame(Guid.NewGuid()));
            return frames;
        }
    }
}