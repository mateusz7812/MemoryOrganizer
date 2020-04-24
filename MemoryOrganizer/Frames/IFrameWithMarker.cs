using System;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Frames
{
    public interface IFrameWithMarker: IFrame
    {
        bool Bit { get; set; }
    }
}