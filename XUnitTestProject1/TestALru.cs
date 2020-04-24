using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTestProject1
{
    public class TestALru
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TestALru(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        class UpdatingFrame : IFrameWithMarker
        {
            private IPage _page;
            public object Clone() => MemberwiseClone();
            public Guid Id { get; } = Guid.NewGuid();

            public IPage Page
            {
                get => _page;
                set
                {
                    Bit = true;
                    _page = value;
                }
            }

            public Guid PageId => _page?.Id ?? Guid.Empty;

            public bool HavePage(Guid id)
            {
                var b = PageId.Equals(id);
                if (b) Bit = true;
                return b;
            }

            public bool Bit { get; set; } = false;
        }

        [Fact]
        public void Test1()
        {
            var frames = new List<IFrameWithMarker>()
            {
                new UpdatingFrame(),
                new UpdatingFrame(),
                new UpdatingFrame(),
                new UpdatingFrame(),
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
                Mock.Of<IPageReference>(m=>m.PageId == pages[0].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[1].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[2].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[3].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[4].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[2].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[0].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[4].Id),
                Mock.Of<IPageReference>(m=>m.PageId == pages[1].Id),
            };
            var correctFramesReferences = new List<Guid>()
            {
                frames[0].Id,
                frames[1].Id,
                frames[2].Id,
                frames[3].Id,
                frames[0].Id,
                frames[1].Id,
                frames[3].Id,
            };
            var physicalMemory = Mock.Of<IPhysicalMemory>();
            Mock.Get(physicalMemory).Setup(m => m.GetEnumerator()).Returns(frames.GetEnumerator());
            var strategy = new ALruStrategy(physicalMemory);
            var frameIdEnum = correctFramesReferences.GetEnumerator();
            foreach (var pageReference in pageReferences)
            {
                _testOutputHelper.WriteLine("-----");
                _testOutputHelper.WriteLine((pages.IndexOf(pages.Find(p=>p.Id.Equals(pageReference.PageId))) + 1).ToString());
                

                var frame = frames.FirstOrDefault(f=>f.HavePage(pageReference.PageId));
                if (frame == null)
                {
                    var correctPageId = pageReference.PageId;
                    frameIdEnum.MoveNext();
                    var correctFrameId = frameIdEnum.Current;

                    var visitor = strategy.CreateReplacingVisitor(pageReference);

                    Assert.Equal(correctPageId, visitor.PageId);
                    Assert.Equal(correctFrameId, visitor.FrameId);
                    Assert.Null(visitor.Page);

                    frames.Find(f => f.Id.Equals(visitor.FrameId)).Page = pages.Find(f => f.Id == visitor.PageId);
                }

                foreach (var f in frames)
                {
                    _testOutputHelper.WriteLine((frames.IndexOf(f)+1) + " " + (pages.IndexOf(f.Page) +1) + " " + f.Bit);
                }
                foreach (var f in strategy._queue)
                {
                    _testOutputHelper.WriteLine((strategy._queue.IndexOf(f)+1) + " " + (pages.IndexOf(f.Page) +1));
                }
                _testOutputHelper.WriteLine("-----");
            }
            frameIdEnum.Dispose();
            Assert.Equal(correctFramesReferences.Count, strategy.PagesErrors);
        }
    }
}
