using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer.Pages;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.Memories
{
    class VirtualMemory: IVirtualMemory
    {
        private readonly List<IPage> _pageList;

        public VirtualMemory(List<IPage> pageList) => 
            _pageList = pageList;

        public void Accept(IReplacingVisitor visitor) => 
            visitor.Page = _pageList.First(p => p.Id.Equals(visitor.PageId));
    }
}
