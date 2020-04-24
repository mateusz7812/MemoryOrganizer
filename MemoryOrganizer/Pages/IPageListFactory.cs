using System.Collections.Generic;

namespace MemoryOrganizer.Pages
{
    interface IPageListFactory
    {
        List<IPage> CreateList(int amount);
    }
}
