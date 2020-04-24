using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Frames
{
    class Frame: IFrame
    {
        public Frame(Guid id) => Id = id;

        public object Clone() => MemberwiseClone();

        public Guid Id { get; }

        public IPage Page { get; set; }

        public Guid PageId => Page?.Id ?? Guid.Empty;

        public bool HavePage(Guid id)
        {
            return Page != null && PageId.Equals(id);
        }
    }
}
