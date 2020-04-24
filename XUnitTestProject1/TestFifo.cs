using System;
using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using Moq;
using Xunit;

namespace XUnitTestProject1
{
    public class TestFifo
    {
        [Fact]
        public void Test1()
        {
            var frames = new List<IFrame>()
            {
                Mock.Of<IFrame>(m=>m.Id == Guid.NewGuid()),
                Mock.Of<IFrame>(m=>m.Id == Guid.NewGuid()),
                Mock.Of<IFrame>(m=>m.Id == Guid.NewGuid()),
                Mock.Of<IFrame>(m=>m.Id == Guid.NewGuid()),
            };
            var pages = new List<IPage>()
            {
                Mock.Of<IPage>(m=>m.Id == Guid.NewGuid()),
                Mock.Of<IPage>(m=>m.Id == Guid.NewGuid()),
                Mock.Of<IPage>(m=>m.Id == Guid.NewGuid()),
                Mock.Of<IPage>(m=>m.Id == Guid.NewGuid()),
                Mock.Of<IPage>(m=>m.Id == Guid.NewGuid()),
            };
            var pageReferences = new List<IPageReference>()
            {
                Mock.Of<IPageReference>(m => m.PageId == pages[0].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[1].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[2].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[3].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[0].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[1].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[4].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[0].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[1].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[2].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[3].Id),
                Mock.Of<IPageReference>(m => m.PageId == pages[4].Id),
            };
            var correctFramesReferences = new List<Guid>()
            {
                frames[0].Id,
                frames[1].Id,
                frames[2].Id,
                frames[3].Id,
                frames[0].Id,
                frames[1].Id,
                frames[2].Id,
                frames[3].Id,
                frames[0].Id,
                frames[1].Id,
            };
            var physicalMemory = Mock.Of<IPhysicalMemory>();
            Mock.Get(physicalMemory).Setup(m => m.GetEnumerator()).Returns(frames.GetEnumerator());
            var strategy = new FifoStrategy(physicalMemory);
            var frameIdEnum = correctFramesReferences.GetEnumerator();
            foreach (var pageReference in pageReferences)
            {
                if (frames.Where(f => f.Page != null).Select(f => f.Page.Id).Contains(pageReference.PageId))
                {
                    continue;
                }
                var correctPageId = pageReference.PageId;
                frameIdEnum.MoveNext();
                var correctFrameId = frameIdEnum.Current;

                var visitor = strategy.CreateReplacingVisitor(pageReference);

                Assert.Equal(correctPageId, visitor.PageId);
                Assert.Equal(correctFrameId, visitor.FrameId);
                Assert.Null(visitor.Page);

                frames.Find(f => f.Id.Equals(visitor.FrameId)).Page = pages.Find(f => f.Id == visitor.PageId);
            }
            Assert.Equal(10, strategy.PagesErrors);
        }
    }
}