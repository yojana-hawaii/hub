using hub.dal.repository.directory;
using hub.domain.model.directory;
using hub.InMemoryTests.Seed;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hub.InMemoryTests.Directory
{
    [TestFixture]
    class InMemory_LocationRepositoryTests : HubDbContextBase
    {
        private LocationRepository _locObj;
        //using tuples
        private (int, string) itBuilding;
        private (int, string) acctBuilding;
        private (int, string) remote;
        private (int, string) execBuilding;
        private (int, string) newBuilding;
        private (int, string) _invalid;
        private int buildingCount;
        private int remoteCount;
        private (int, string) user;

        [SetUp]
        public void Setup()
        {
            _locObj = new LocationRepository(_context);
            itBuilding = (1, "IT Building");
            acctBuilding = (2, "Accounting Building");
            remote = (3, "Remove Worker");
            execBuilding = (4, "Exec Building");
            newBuilding = (5, "New Building");
            _invalid = (-1, "");
            buildingCount = 5;
            remoteCount = 3;
            user = (5, "rm");
        }


        [Test]
        public void GetAllLocations_Valid_ReturnsLocationList()
        {
            IEnumerable<Location> _loc = _locObj.GetAllLocations();

            Assert.IsNotNull(_loc);
            Assert.That(buildingCount, Is.EqualTo(_loc.Count()));
            CollectionAssert.AllItemsAreUnique(_loc);
            CollectionAssert.AllItemsAreNotNull(_loc);

            Assert.That(_loc.Count(l => l.LocationName == itBuilding.Item2), Is.EqualTo(1));

            var oneLoc = _loc.FirstOrDefault(l => l.LocationId == itBuilding.Item1);
            Assert.AreEqual(oneLoc.LocationName, itBuilding.Item2);
        }


        [Test]
        public void GetByLocationId_ValidId_ReturnOneLocation()
        {
            Location loc1 = _locObj.GetByLocationId(execBuilding.Item1);
            Location loc2 = _locObj.GetByLocationId(acctBuilding.Item1);

            Assert.IsNotNull(loc1);
            Assert.IsNotNull(loc2);

            Assert.AreEqual(execBuilding.Item2, loc1.LocationName);
            Assert.AreEqual(acctBuilding.Item2, loc2.LocationName);
        }
        [Test]
        public void GetByLocationId_InvalidId_ReturnOneNullLocation()
        {
            var loc = _locObj.GetByLocationId(_invalid.Item1);

            Assert.IsNotNull(loc);

            Assert.AreEqual(_invalid.Item2, loc.LocationName);
        }


        [Test]
        public void GetEmployeesByLocation_ValidId_ReturnEmployeesList()
        {
            IEnumerable<Employee> emps = _locObj.GetEmployeesByLocation(remote.Item1);

            Assert.IsNotNull(emps);
            Assert.That(remoteCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);

            Assert.That(emps.Count(l => l.Username == user.Item2), Is.EqualTo(1));

            var oneEmp = emps.FirstOrDefault(e => e.EmployeeId == user.Item1);
            Assert.That(oneEmp.Username, Is.EqualTo(user.Item2));
        }
        [Test]
        public void GetEmployeesByLocation_InvalidId_ReturnNullEmployeeList()
        {
            var emps = _locObj.GetEmployeesByLocation(_invalid.Item1);

            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            var nullEmp = emps.FirstOrDefault(e => e.EmployeeId == _invalid.Item1);
            Assert.That(nullEmp.EmployeeId, Is.EqualTo(_invalid.Item1));
            Assert.That(nullEmp.Username, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetEmployeeByLocation_MissingEmployee_ReturnNullEmployeeList()
        {
            var emps = _locObj.GetEmployeesByLocation(newBuilding.Item1);

            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            var nullEmp = emps.FirstOrDefault(e => e.EmployeeId == _invalid.Item1);
            Assert.That(nullEmp.Username, Is.EqualTo(_invalid.Item2));
        }


        [Test]
        public void GetLocationId_ValidName_ReturnOneLocID()
        {
            int locId = _locObj.GetLocationId(execBuilding.Item2);
            Assert.AreEqual(execBuilding.Item1, locId);
        }
        [Test]
        public void GetLocationId_InvalidName_ReturnOneNullJobId()
        {
            var locId = _locObj.GetLocationId(_invalid.Item2);
            Assert.AreEqual(_invalid.Item1, locId);
        }
    }
}
