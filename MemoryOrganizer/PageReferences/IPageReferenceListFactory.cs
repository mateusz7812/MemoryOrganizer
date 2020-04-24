using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.PageReferences
{
    interface IPageReferenceListFactory
    {
        List<IPageReference> CreateList(List<IPage> pages, int amount);
    }
}
