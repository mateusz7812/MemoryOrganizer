using System;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Frames
{
    public class UniversalFrame: IFrameWithTimeStamps, IFrameWithMarker
    {
        private IPage _page;

        public UniversalFrame(Guid id)
        {
            Id = id;
        }

        public object Clone() => MemberwiseClone();

        public Guid Id { get; }

        public IPage Page
        {
            get => _page;
            set
            {
                LastUsage = DateTime.Now;
                Bit = true;
                _page = value;
            }
        }

        public Guid PageId => _page?.Id ?? Guid.Empty;
        public bool HavePage(Guid id)
        {
            var havePage = PageId.Equals(id);
            if (!havePage) return false;
            LastUsage = DateTime.Now;
            Bit = true;
            return true;
        }

        public DateTime LastUsage { get; private set; }
        public bool Bit { get; set; } = false;
    }
}