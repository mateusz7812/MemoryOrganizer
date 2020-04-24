using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOrganizer.Pages
{
    class PageListFactory : IPageListFactory
    {
        public List<IPage> CreateList(int amount)
        {
            var pages = new List<IPage>();
            for (var i = 0; i < amount; i++)
                pages.Add(new Page(Guid.NewGuid()));
            return pages;
        }
    }
}