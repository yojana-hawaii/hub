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
        private (int, string, string, string) it;
        private (int, string) _invalid;
        private (int, string, string) emptyDeptLocJob;
        

        [SetUp]
        public void Setup()
        {
            _faxObj = new FaxNumberRepository(_context);
            faxCount = 3;
            it = (1, "125-8956", "IT", "IT Building");
            _invalid = (-1, "");
            emptyDeptLocJob = (12, "x", "");
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
        public void GetFaxNumberById_ValidId_ReturnOneEmployee()
        {
            FaxNumber fax = _faxObj.GetFaxNumberById(it.Item1);

            Assert.AreEqual(it.Item2, fax.Number);
        }
        [Test]
        public void GetFaxNumberById_InValidId_ReturnOneNullEmployee()
        {
            var fax = _faxObj.GetFaxNumberById(_invalid.Item1);

            Assert.IsNotNull(fax);

            Assert.AreEqual(_invalid.Item2, fax.Number);
        }



        [Test]
        public void GetFaxDepartment_ValidId_ReturnOneDepartment()
        {
            string dept = _faxObj.GetFaxDepartment(it.Item1);

            Assert.IsNotNull(dept);
            Assert.That(dept, Is.EqualTo(it.Item3));
        }
        [Test]
        public void GetFaxDepartment_InvalidId_ReturnOneNullDepartment()
        {
            string dept = _faxObj.GetFaxDepartment(_invalid.Item1);
            Assert.IsNotNull(dept);
            Assert.That(dept, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetFaxDepartment_MissingDept_ReturnOneNullDepartment()
        {
            string dept = _faxObj.GetFaxDepartment(emptyDeptLocJob.Item1);
            Assert.AreEqual(dept, emptyDeptLocJob.Item3);
        }

        [Test]
        public void GetFaxLocation_validId_ReturnOneLocation()
        {
            string loc = _faxObj.GetFaxLocation(it.Item1);

            Assert.IsNotNull(loc);
            Assert.That(loc, Is.EqualTo(it.Item4));
        }
        [Test]
        public void GetFaxLocation_InvalidId_ReturnOneNullLocation()
        {
            string loc = _faxObj.GetFaxLocation(_invalid.Item1);
            Assert.IsNotNull(loc);
            Assert.That(loc, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetFaxLocation_MissingLoc_ReturnOneNullLocation()
        {
            string loc = _faxObj.GetFaxLocation(emptyDeptLocJob.Item1);
            Assert.AreEqual(loc, emptyDeptLocJob.Item3);
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
