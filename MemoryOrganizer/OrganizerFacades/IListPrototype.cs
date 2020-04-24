using System;
using System.Collections.Generic;

namespace MemoryOrganizer.OrganizerFacades
{
    interface IListPrototype<T> where T: ICloneable
    {
        void Set(List<T> list);
        List<T> Clone();
    }
}
