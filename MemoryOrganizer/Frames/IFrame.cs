using System;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Frames
{
    public interface IFrame: ICloneable
    {
        Guid Id { get; }
        IPage Page { get; set; }
        Guid PageId { get; }
        bool HavePage(Guid id);
    }
}
