using System;
using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.Processes
{
    public class Process: IProcess
    {
        private readonly IEnumerator<IReplacingVisitor> _pagesToLoad;

        public Process(IStrategyFactory strategyFactory, List<IPageReference> pageReferenceList)
        {
            PageReferenceList = pageReferenceList;
            Strategy = strategyFactory.Create(this);
            _pagesToLoad = GetPagesEnumerable().GetEnumerator();
        }

        public IPhysicalMemory PhysicalMemory { get; set; }
        public IVirtualMemory VirtualMemory { get; set; }
        public IOrganizingStrategy Strategy { get; set; }
        public string StrategyName => Strategy.Name;
        public List<IPageReference> PageReferenceList { get; }
        public int PagesErrors { get; private set; } = 0;
        public bool HasNextPageToLoad => _pagesToLoad.Current != null;

        private IEnumerable<IReplacingVisitor> GetPagesEnumerable()
        {
            foreach (var replacing in from pageReference 
                in PageReferenceList
                where !PhysicalMemory.Contains(pageReference.PageId)
                select Strategy.CreateReplacingVisitor(pageReference))
            {
                yield return replacing;
            }

            PagesErrors = Strategy.PagesErrors;
        }

        public void LoadNextPage()
        {
            var replacing = _pagesToLoad.Current;
            if (replacing==null) throw new NullReferenceException("no more pages to load");
            VirtualMemory.Accept(replacing);
            PhysicalMemory.Accept(replacing);
            _pagesToLoad.MoveNext();
        }
    }
}