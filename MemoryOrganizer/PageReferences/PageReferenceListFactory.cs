using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.PageReferences
{
    class PageReferenceListFactory : IPageReferenceListFactory
    {
        public List<IPageReference> CreateList(List<IPage> pages, int amount)
        {
            var references = new List<IPageReference>();
            for (var i = 0; i < amount; i++)
                references.Add(new PageReference(RandomPageId(pages)));
            return references;
        }

        private static Guid RandomPageId(List<IPage> pages)
        {
            var provider = new RNGCryptoServiceProvider();
            var box = new byte[1];
            var pagesCount = pages.Count;
            do provider.GetBytes(box);
            while (!(box[0] < pagesCount * (Byte.MaxValue / pagesCount)));
            var index = (box[0] % pagesCount);
            return pages[index].Id;
        }
    }
}