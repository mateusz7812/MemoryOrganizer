using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class OptStrategy : IOrganizingStrategy
    {
        private readonly IPhysicalMemory _physicalMemory;
        private readonly List<IPageReference> _pageReferences;

        public OptStrategy(IPhysicalMemory physicalMemory, List<IPageReference> pageReferences)
        {
            _physicalMemory = physicalMemory;
            _pageReferences = pageReferences;
        }

        public string Name => "Opt";
        public int PagesErrors { get; private set; } = 0;

        public IReplacingVisitor CreateReplacingVisitor(IPageReference pageReference)
        {
            PagesErrors++;
            var pageId = pageReference.PageId;
            var frameId = GetTheMostUselessFrame(pageReference).Id;
            return new ReplacingVisitor(pageId, frameId);
        }

        private IFrame GetTheMostUselessFrame(IPageReference pageReference)
        {
            var dict = new Dictionary<IFrame, int>();
            foreach (var frame in _physicalMemory) dict[frame] = 0;
            var futureReferences = _pageReferences.Select(item => item).ToList();
            futureReferences.RemoveRange(0, futureReferences.IndexOf(pageReference));
            var referencesCount = _pageReferences.Count;
            foreach (var frame in dict.Keys.ToList())
            {
                if (frame.Page == null)
                {
                    dict[frame] = referencesCount;
                    break;
                }
                var index = futureReferences.FindIndex(r => r.PageId.Equals(frame.Page.Id));
                if (index == -1)
                {
                    dict[frame] = referencesCount;
                    break;
                }
                dict[frame] = index;
            }

            var uselessFrame = dict.OrderByDescending(d => d.Value).First().Key;
            return uselessFrame;
        }
    }
}
