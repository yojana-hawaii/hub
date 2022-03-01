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
    class InMemoryEmployeeRepository : HubDbContextBase
    {
        private EmployeeRepository _empObj;
        private int empCount;
        private (int, string, string) ceo;
        private (int, string, string, string, string, string) jc;
        private (int, string, string, string) dc;
        private (int, string, string, string, int, string) ls;
        private (int, string, string, string, string, string, int, string, string) empX;
        private (int, string) missingFirstName;
        private (int, string) missingFirstAndLastName;
        private (int, string, string) _invalid;
        private (int, string, string) emptyDeptLocJob;



        [SetUp]
        public void Setup()
        {
            _empObj = new EmployeeRepository(_context);
            empCount = 13;
            ceo = (1, "ek", "ek@email.com");
            jc = (9, "jc", "l", "c, j", "123-9876", "123");
            dc = (5, "dc", "dc@email.com", "Accounting");
            ls = (2, "ls", "Exec Building", "CFO", 4, "963-8520");
            _invalid = (-1, "", "xx");
            emptyDeptLocJob = (12, "x", "");
            missingFirstName = (11, "l");
            missingFirstAndLastName = (7, "");
            empX = (3, "am", "ls", "Systems Administrator", "IT Building", "IT", 3, "p", "987");
        }

      
        [Test]
        public void GetAllEmployee_Valid_ReturnEmployeeList()
        {
            IEnumerable<Employee> emps = _empObj.GetAllEmployees();

            Assert.IsNotNull(emps);
            Assert.That(empCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);
            Assert.That(emps.Count(e => e.Username == ceo.Item2), Is.EqualTo(1));

            var oneEmp = emps.FirstOrDefault(e => e.EmployeeId == jc.Item1);
            Assert.AreEqual(oneEmp.Username, jc.Item2);
        }


        [Test]
        public void GetEmployeeById_ValidId_ReturnOneEmployee()
        {
            Employee emp = _empObj.GetEmployeeById(ceo.Item1);

            Assert.AreEqual(ceo.Item3, emp.Email);
        }
        [Test]
        public void GetEmployeeById_InValidId_ReturnOneNullEmployee()
        {
            var emp = _empObj.GetEmployeeById(_invalid.Item1);

            Assert.IsNotNull(emp);

            Assert.AreEqual(_invalid.Item2, emp.Username);
        }


        [Test]
        public void GetFullname_ValidId_ReturnFullname()
        {
            var fullname = _empObj.GetFullName(jc.Item1);
            Assert.That(jc.Item4, Is.EqualTo(fullname));
        }
        [Test]
        public void GetFullname_NoLastname_ReturnFirstName()
        {
            var fullname = _empObj.GetFullName(emptyDeptLocJob.Item1);
            Assert.That(emptyDeptLocJob.Item2, Is.EqualTo(fullname));
        }
        [Test]
        public void GetFullname_NoFirstname_ReturnLastName()
        {
            var fullname = _empObj.GetFullName(missingFirstName.Item1);
            Assert.That(missingFirstName.Item2, Is.EqualTo(fullname));
        }
        [Test]
        public void GetFullname_NoFirstOrLastName_ReturnBlank()
        {
            var fullname = _empObj.GetFullName(missingFirstAndLastName.Item1);
            Assert.AreEqual(missingFirstAndLastName.Item2, fullname);
        }
        [Test]
        public void GetFullname_InvalidId_ReturnOneNullEmployee()
        {
            var fullName = _empObj.GetFullName(_invalid.Item1);
            Assert.IsNotNull(fullName);

            Assert.AreEqual(_invalid.Item2, fullName);
        }


        [Test]
        public void GetEmployeeDepartment_ValidId_ReturnOneDepartment()
        {
            string dept = _empObj.GetEmployeeDepartment(dc.Item1);

            Assert.IsNotNull(dept);
            Assert.That(dept, Is.EqualTo(dc.Item4));
        }
        [Test]
        public void GetEmployeeDepartment_InvalidId_ReturnOneNullDepartment()
        {
            string dept = _empObj.GetEmployeeDepartment(_invalid.Item1);
            Assert.IsNotNull(dept);
            Assert.That(dept, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetEmployeeDepartment_MissingDept_ReturnOneNullDepartment()
        {
            string dept = _empObj.GetEmployeeDepartment(emptyDeptLocJob.Item1);
            Assert.AreEqual(dept, emptyDeptLocJob.Item3);
        }


        [Test]
        public void GetEmployeeJobTitle_ValidId_ReturnOneJobTitle()
        {
            string job = _empObj.GetEmployeeJobTitle(ls.Item1);

            Assert.IsNotNull(job);
            Assert.That(job, Is.EqualTo(ls.Item4));
        }
        [Test]
        public void GetEmployeeJobTitle_InvalidId_ReturnOneNullJobTitle()
        {
            string job = _empObj.GetEmployeeJobTitle(_invalid.Item1);
            Assert.IsNotNull(job);
            Assert.That(job, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetEmployeeJobTitle_MissingJob_ReturnOneNullJobTitiel()
        {
            string job = _empObj.GetEmployeeJobTitle(emptyDeptLocJob.Item1);
            Assert.AreEqual(job, emptyDeptLocJob.Item3);
        }


        [Test]
        public void GetEmployeeLocation_validId_ReturnOneLocation()
        {
            string loc = _empObj.GetEmployeeLocation(ls.Item1);

            Assert.IsNotNull(loc);
            Assert.That(loc, Is.EqualTo(ls.Item3));
        }
        [Test]
        public void GetEmployeeLocation_InvalidId_ReturnOneNullLocation()
        {
            string loc = _empObj.GetEmployeeLocation(_invalid.Item1);
            Assert.IsNotNull(loc);
            Assert.That(loc, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetEmployeeLocation_MissingLoc_ReturnOneNullLocation()
        {
            string loc = _empObj.GetEmployeeLocation(emptyDeptLocJob.Item1);
            Assert.AreEqual(loc, emptyDeptLocJob.Item3);
        }


        [Test]
        public void GetEmployeeByADUserName_ValidUsername_ReturnOneEmployee()
        {
            //empX = (3, "am", "ls", "Systems Administrator", "IT Building", "IT", 2, "a");
            var emp = _empObj.GetEmployeeByADUserName(empX.Item2);
            var staff = emp.PrimaryStaff.FirstOrDefault(s => s.Username == "pm");

            Assert.IsNotNull(emp);
            CollectionAssert.AllItemsAreNotNull(emp.PrimaryStaff);
            CollectionAssert.AllItemsAreUnique(emp.PrimaryStaff);


            Assert.That(empX.Item3, Is.EqualTo(emp.PrimaryManager.Username));
            Assert.That(empX.Item4, Is.EqualTo(emp.JobTitle.JobTitleName));
            Assert.That(empX.Item5, Is.EqualTo(emp.Location.LocationName));
            Assert.That(empX.Item6, Is.EqualTo(emp.Department.DepartmentName));
            Assert.That(empX.Item7, Is.EqualTo(emp.PrimaryStaff.Count()));
            Assert.That(empX.Item8, Is.EqualTo(staff.FirstName));
        }
        [Test]
        public void GetEmployeeByADUserName_InvalidUsername_ReturnOneNullEmployee()
        {
            var emp = _empObj.GetEmployeeByADUserName(_invalid.Item3);

            Assert.IsNotNull(emp);
            Assert.AreEqual(_invalid.Item1, emp.EmployeeId);
            Assert.AreEqual(_invalid.Item2, emp.Username);
        }


        [Test]
        public void GetEmployeePhone_ValidId_ReturnsFullAndExtension()
        {
            string expected = jc.Item6 + " -> " + jc.Item5;
            string returned = _empObj.GetEmployeePhone(jc.Item1);
            Assert.AreEqual(expected, returned);
        }
        [Test]
        public void GetEmployeePhone_ValidId_ReturnOnlyExtension()
        {
            string phone = _empObj.GetEmployeePhone(empX.Item1);
            Assert.AreEqual(empX.Item9, phone);
        }
        [Test]
        public void GetEmployeePhone_ValidId_ReturnOnlyFullNumber()
        {
            string phone = _empObj.GetEmployeePhone(ls.Item1);
            Assert.AreEqual(ls.Item6, phone);
        }
        [Test]
        public void GetEmployeePhone_InvalidId_ReturnOneNullEmployeePhone()
        {
            var phone = _empObj.GetEmployeePhone(_invalid.Item1);
            Assert.IsNotNull(phone);
            Assert.AreEqual(_invalid.Item2, phone);
        }




        [Test]
        public void GetManager_ValidId_ReturnOneManager()
        {
            Employee manager = _empObj.GetManager(jc.Item1);
            Assert.IsNotNull(manager);
            Assert.That(manager.FirstName, Is.EqualTo(jc.Item3));
        }
        [Test]
        public void GetManager_InvalidId_ReturnOneNullManager()
        {
            var manager = _empObj.GetManager(_invalid.Item1);
            Assert.IsNotNull(manager);
            Assert.AreEqual(manager.Username, _invalid.Item2);
        }
        [Test]
        public void GetManager_MissingManager_ReturnNullManager()
        {
            var manager = _empObj.GetManager(ceo.Item1);
            Assert.IsNotNull(manager);
            Assert.AreEqual(manager.Username, _invalid.Item2);
        }


        [Test]
        public void GetStaffs_ValidId_ReturnStaffList()
        {
            var staff = _empObj.GetStaffs(ls.Item1);
            Assert.IsNotNull(staff);
            Assert.That(ls.Item5, Is.EqualTo(staff.Count()));
            CollectionAssert.AllItemsAreNotNull(staff);
            CollectionAssert.AllItemsAreUnique(staff);
            Assert.That(staff.Count(s => s.Username == jc.Item2), Is.EqualTo(1));
        }
        [Test]
        public void GetStaffs_InvalidId_ReturnNulStaffList()
        {
            var staff = _empObj.GetStaffs(_invalid.Item1);

            Assert.IsNotNull(staff);
            Assert.That(1, Is.EqualTo(staff.Count()));
            CollectionAssert.AllItemsAreNotNull(staff);
            CollectionAssert.AllItemsAreUnique(staff);


        }
        [Test]
        public void GetStaffs_MissingStaff_ReturnNullStaffList()
        {
            var staff = _empObj.GetStaffs(jc.Item1);
            Assert.IsEmpty(staff);
        }


      
        [Test]
        public void GetEmployeeByKeyword_ValidOneWord_ReturnEmployees()
        {
            var emps = _empObj.GetEmployeeByKeywordSearch("ls");
            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
        [Test]
        public void GetEmployeeByKeyword_ValidTwoWord_ReturnEmployees()
        {
            var emps = _empObj.GetEmployeeByKeywordSearch("ls am");
            Assert.IsNotNull(emps);
            Assert.That(3, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
        [Test]
        public void GetEmployeeByKeyword_ValidThreeWord_ReturnEmployees()
        {
            var emps = _empObj.GetEmployeeByKeywordSearch("ls am jc");
            Assert.IsNotNull(emps);
            Assert.That(4, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
        [Test]
        public void GetEmployeeByKeyword_ValidOneWord_ReturnMultipleEmployees()
        {

            var emps = _empObj.GetEmployeeByKeywordSearch("am");
            Assert.IsNotNull(emps);
            Assert.That(2, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
        [Test]
        public void GetEmployeeByKeywork_ValidDigits_ReturnEmployees()
        {
            var emps = _empObj.GetEmployeeByKeywordSearch("520");
            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
        [Test]
        public void GetEmployeeByKeyword_EmptyString_ReturnAllEmployees()
        {
            var emps = _empObj.GetEmployeeByKeywordSearch(" ");
            Assert.IsNotNull(emps);
            Assert.That(empCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
        [Test]
        public void GetEmployeeByKeyword_OnlySpecialCharacters_ReturnAllEmployees()
        {
            var emps = _empObj.GetEmployeeByKeywordSearch("%.");
            Assert.IsNotNull(emps);
            Assert.That(empCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
        [Test]
        public void GetEmployeeByKeyword_SpecialCharactersMixedWithLetter_ReturnEmployees()
        {
            var emps = _empObj.GetEmployeeByKeywordSearch("%am*^");
            Assert.IsNotNull(emps);
            Assert.That(2, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
        }
    }
}
