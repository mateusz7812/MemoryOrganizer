using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOrganizer.Pages
{
    class Page: IPage
    {
        public Page(Guid id)
        {
            Id = id;
        }

        public object Clone() => MemberwiseClone();

        public Guid Id { get; }
    }
}
