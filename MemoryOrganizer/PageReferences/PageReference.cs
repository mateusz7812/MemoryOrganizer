using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.PageReferences
{
    class PageReference: IPageReference
    {
        public PageReference(Guid pageId)
        {
            PageId = pageId;
        }

        public object Clone() => 
            MemberwiseClone();
        
        public Guid PageId { get; }
    }
}
