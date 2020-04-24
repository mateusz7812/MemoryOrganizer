using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Memories;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class FifoStrategy : IOrganizingStrategy
    {
        private readonly List<Guid> _frameGuidList = new List<Guid>();
        private readonly IEnumerator<Guid> _enumerator;

        public FifoStrategy(IPhysicalMemory physicalMemory)
        {
            foreach (var frame in physicalMemory)
                _frameGuidList.Add(frame.Id);
            _enumerator = RepeatForever(_frameGuidList).GetEnumerator();
        }

        private static IEnumerable<T> RepeatForever<T>(IEnumerable<T> sequence)
        {
            while (true)
                foreach (var item in sequence)
                    yield return item;
        }

        public string Name => "Fifo";
        public int PagesErrors { get; private set; } = 0;

        public IReplacingVisitor CreateReplacingVisitor(IPageReference pageReference)
        {
            PagesErrors++;
            _enumerator.MoveNext();

            var pageId = pageReference.PageId;
            var frameId = _enumerator.Current;
            return new ReplacingVisitor(pageId, frameId);
        }
    }
}