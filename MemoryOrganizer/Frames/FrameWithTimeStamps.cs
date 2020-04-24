using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Frames
{
    class FrameWithTimeStamps: IFrameWithTimeStamps
    {
        private IPage _page;
        public DateTime LastUsage { get; private set; }
        public Guid Id { get; }

        public IPage Page
        {
            get => _page;
            set
            {
                LastUsage = DateTime.Now;
                _page = value;
            }
        }

        public Guid PageId => Page?.Id ?? Guid.Empty;
        public bool HavePage(Guid id)
        {
            var havePage = PageId.Equals(id);
            if(havePage)
                LastUsage = DateTime.Now;
            return havePage;
        }

        public FrameWithTimeStamps(Guid id)
        {
            Id = id;
        }

        public object Clone() => MemberwiseClone();
    }
}
