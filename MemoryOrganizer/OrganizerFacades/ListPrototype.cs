using System;
using System.Collections.Generic;
using System.Linq;

namespace MemoryOrganizer.OrganizerFacades
{
    class ListPrototype<T>: IListPrototype<T> where T: ICloneable
    {
        private List<T> _list;
        public void Set(List<T> list) => _list = list;
        public List<T> Clone() => _list.Select(item => (T)item.Clone()).ToList();
    }
}
