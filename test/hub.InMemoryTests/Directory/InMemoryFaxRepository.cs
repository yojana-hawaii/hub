using hub.dal.repository.directory;
using hub.domain.model.directory;
using hub.InMemoryTests.Seed;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace hub.InMemoryTests.Directory
{
    [TestFixture]
    class InMemoryFaxRepository : HubDbContextBase
    {
        private FaxNumberRepository _faxObj;
        private int faxCount;
        private (int, string) it;

        [SetUp]
        public void Setup()
        {
            _faxObj = new FaxNumberRepository(_context);
            faxCount = 3;
            it = (1, "125-8956");
        }

        [Test]
        public void GetAllFaxNumber_Valid_ReturnFaxList()
        {
            IEnumerable<FaxNumber> faxes = _faxObj.GetAllFaxNumbers();

            Assert.IsNotNull(faxes);
            Assert.AreEqual(faxCount, faxes.Count());
            CollectionAssert.AllItemsAreNotNull(faxes);
            CollectionAssert.AllItemsAreUnique(faxes);

            var oneFax = faxes.FirstOrDefault(f => f.FaxId == it.Item1);
            Assert.AreEqual(oneFax.Number, it.Item2);
        }

        [Test]
        public void GetFaxNumberByKeywordSearch_Valid_ReturnFaxList()
        {
            var faxes = _faxObj.GetFaxNumberByKeywordSearch(it.Item2);

            Assert.IsNotNull(faxes);
            Assert.AreEqual(faxCount, faxes.Count());
            CollectionAssert.AllItemsAreNotNull(faxes);
            CollectionAssert.AllItemsAreUnique(faxes);

            var oneFax = faxes.FirstOrDefault(f => f.Number == it.Item2);
            Assert.AreEqual(oneFax.FaxId, it.Item1);
        }

        [Test]
        public void GetFaxNumberByKeywordSearch_EmptyString_ReturnAllFaxes()
        {
            var faxes = _faxObj.GetFaxNumberByKeywordSearch("  ");

            Assert.IsNotNull(faxes);
            Assert.AreEqual(faxCount, faxes.Count());
            CollectionAssert.AllItemsAreNotNull(faxes);
            CollectionAssert.AllItemsAreUnique(faxes);

            var oneFax = faxes.FirstOrDefault(f => f.FaxId == it.Item1);
            Assert.AreEqual(oneFax.Number, it.Item2);

        }
    }
}
