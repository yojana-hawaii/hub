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
        private (int, string) invalidBuilding;
        private int buildingCount;
        private int remoteCount;
        private (int, string) user;
        private string locErrorMessage;

        [SetUp]
        public void Setup()
        {
            _locObj = new LocationRepository(_context);
            itBuilding = (1, "IT Building");
            acctBuilding = (2, "Accounting Building");
            remote = (3, "Remove Worker");
            execBuilding = (4, "Exec Building");
            invalidBuilding = (99, "Marketing Building");
            buildingCount = 4;
            remoteCount = 3;
            user = (5, "rm");
            locErrorMessage = "Location not found";
        }


        [Test]
        public void GetAllLocations_Valid_ReturnsListOfLocations()
        {
            IEnumerable<Location> _loc = _locObj.GetAllLocations();

            Assert.IsNotNull(_loc);
            Assert.That(buildingCount, Is.EqualTo(_loc.Count()));
            CollectionAssert.AllItemsAreUnique(_loc);
            CollectionAssert.AllItemsAreNotNull(_loc);
            Assert.That(_loc.Count(l => l.LocationName == itBuilding.Item2), Is.EqualTo(1));
        }
        [Test]
        public void GetLocationById_ValidId_ReturnLocationObject()
        {
            Location loc1 = _locObj.GetByLocationId(execBuilding.Item1);
            Location loc2 = _locObj.GetByLocationId(acctBuilding.Item1);
            Assert.AreEqual(execBuilding.Item2, loc1.LocationName);
            Assert.AreEqual(acctBuilding.Item2, loc2.LocationName);
        }
        [Test]
        public void GetLocationById_InvalidId_ReturnArgumentException()
        {
            Assert.That(
                () => _locObj.GetByLocationId(invalidBuilding.Item1)
                ,
                Throws
                .TypeOf<ArgumentException>()
                .With.Property("Message")
                .Matches(locErrorMessage)
                );
        }


        [Test]
        public void GetEmployeeByLocID_ValidId_ReturnEmployees()
        {
            IEnumerable<Employee> loc = _locObj.GetEmployeesByLocation(remote.Item1);

            Assert.IsNotNull(loc);
            Assert.That(remoteCount, Is.EqualTo(loc.Count()));
            CollectionAssert.AllItemsAreNotNull(loc);
            CollectionAssert.AllItemsAreUnique(loc);
            Assert.That(loc.Count(l => l.Username == user.Item2), Is.EqualTo(1));
        }
        [Test]
        public void GetEmployeeByLocId_InvalidId_NullReferenceException()
        {
            Assert.That(
                    () => _locObj.GetEmployeesByLocation(invalidBuilding.Item1)
                    ,
                    Throws
                    .TypeOf<NullReferenceException>()
                    .With
                    .Property("Message")
                    .Matches(locErrorMessage)
                );
        }


        [Test]
        public void GetLocId_ValidName_ReturnLocID()
        {
            int locId = _locObj.GetLocationId(execBuilding.Item2);
            Assert.AreEqual(execBuilding.Item1, locId);
        }

        [Test]
        public void GetLocId_InbalidName_ArgumentException()
        {
            Assert.That(
                   () => _locObj.GetLocationId(invalidBuilding.Item2)
                   ,
                   Throws
                   .TypeOf<ArgumentException>()
                   .With
                   .Property("Message")
                   .Matches(locErrorMessage)
               );
        }
    }
}
