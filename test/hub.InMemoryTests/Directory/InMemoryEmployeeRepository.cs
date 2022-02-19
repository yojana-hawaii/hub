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
        private (int, string) missingLastName;
        private (int, string, string) _invalid;

        private string empErrorMessage;


        [SetUp]
        public void Setup()
        {
            _empObj = new EmployeeRepository(_context);
            empCount = 12;
            ceo = (1, "ek", "ek@email.com");
            jc = (9, "jc", "l", "c, j", "123-9876", "123");
            dc = (5, "dc", "dc@email.com", "Accounting");
            ls = (2, "ls", "Exec Building", "CFO", 4, "963-8520");
            _invalid = (99, "hs", "hs@email.com");
            empErrorMessage = "Employee not found";
            missingLastName = (12, "x");
            empX = (3, "am", "ls", "Systems Administrator", "IT Building", "IT", 2, "a", "987");
        }

      
        [Test]
        public void GetAllEmployee_Valid_ReturnListOfEmployee()
        {
            IEnumerable<Employee> emps = _empObj.GetAllEmployees();

            Assert.IsNotNull(emps);
            Assert.That(empCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);
            Assert.That(emps.Count(e => e.Username == ceo.Item2), Is.EqualTo(1));
        }


        [Test]
        public void GetEmpById_ValidId_ReturnEmpObject()
        {
            Employee emp = _empObj.GetEmployeeById(ceo.Item1);

            Assert.AreEqual(ceo.Item3, emp.Email);
        }
        [Test]
        public void GetEmpById_InValidId_ArgumentException()
        {
            Assert.That(
                    () => _empObj.GetEmployeeById(_invalid.Item1)
                    ,
                    Throws
                    .TypeOf<ArgumentException>()
                    .With
                    .Property("Message")
                    .Matches(empErrorMessage)
                );
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
            var fullname = _empObj.GetFullName(missingLastName.Item1);
            Assert.That(missingLastName.Item2, Is.EqualTo(fullname));
        }
        [Test]
        public void GetFullname_InvalidId_NullReferenceException()
        {
            Assert.That(
                    () => _empObj.GetFullName(_invalid.Item1)
                    ,
                    Throws
                    .TypeOf<NullReferenceException>()
                    .With
                    .Property("Message")
                    .Matches(empErrorMessage)
                );
        }


        [Test]
        public void GetDepartmentByEmpId_ValidId_ReturnDepartments()
        {
            string dept = _empObj.GetEmployeeDepartment(dc.Item1);

            Assert.IsNotNull(dept);
            Assert.That(dept, Is.EqualTo(dc.Item4));
        }
        [Test]
        public void GetDeptartmentByEmpId_InvalidId_ReturnBlank()
        {
            // If I initialize here, exception thrown right and test fails
            // So call in assert

            string dept = _empObj.GetEmployeeDepartment(_invalid.Item1);
            Assert.IsNotNull(dept);
            Assert.That(dept, Is.EqualTo(""));
        }


        [Test]
        public void GetJobByEmpId_ValidId_ReturnJob()
        {
            string job = _empObj.GetEmployeeJobTitle(ls.Item1);

            Assert.IsNotNull(job);
            Assert.That(job, Is.EqualTo(ls.Item4));
        }
        [Test]
        public void GetJobByEmpId_InvalidId_ReturnBlank()
        {
            string job = _empObj.GetEmployeeJobTitle(_invalid.Item1);
            Assert.IsNotNull(job);
            Assert.That(job, Is.EqualTo(""));
        }


        [Test]
        public void GetLocationByEmpId_validId_returnLocation()
        {
            string loc = _empObj.GetEmployeeLocation(ls.Item1);

            Assert.IsNotNull(loc);
            Assert.That(loc, Is.EqualTo(ls.Item3));
        }
        [Test]
        public void GetLocationByEmpId_InvalidId_NullReferenceException()
        {
            string loc = _empObj.GetEmployeeLocation(_invalid.Item1);
            Assert.IsNotNull(loc);
            Assert.That(loc, Is.EqualTo(""));
        }


        [Test]
        public void GetManagerByEmpId_ValidId_ReturnManager()
        {
            Employee manager = _empObj.GetManager(jc.Item1);
            Assert.IsNotNull(manager);
            Assert.That(manager.FirstName, Is.EqualTo(jc.Item3));
        }
        [Test]
        public void GetManagerByEmpId_InvalidId_NullReferenceException()
        {
            Assert.That(
                () => _empObj.GetManager(_invalid.Item1)
                ,
                Throws
                .TypeOf<NullReferenceException>()
                .With
                .Property("Message")
                .Matches(empErrorMessage)
                );
        }
        [Test]
        public void GetManagerByEmpId_ValidIdNoManager_ReturnNull()
        {
            var manager = _empObj.GetManager(ceo.Item1);
            Assert.IsNull(manager);
        }


        [Test]
        public void GetStaffByManagerId_ValidId_ReturnStaff()
        {
            var staff = _empObj.GetStaffs(ls.Item1);
            Assert.IsNotNull(staff);
            Assert.That(ls.Item5, Is.EqualTo(staff.Count()));
            CollectionAssert.AllItemsAreNotNull(staff);
            CollectionAssert.AllItemsAreUnique(staff);
            Assert.That(staff.Count(s => s.Username == jc.Item2), Is.EqualTo(1));
        }
        [Test]
        public void GetStaffByManagerId_InvalidId_NullReferenceException()
        {
            Assert.That(
                    () => _empObj.GetStaffs(_invalid.Item1)
                    ,
                    Throws
                    .TypeOf<NullReferenceException>()
                    .With
                    .Property("Message")
                    .Matches(empErrorMessage)
                );
        }
        [Test]
        public void GetStaffByManagerId_ValidIdNoStaff_ReturnEmpty()
        {
            var staff = _empObj.GetStaffs(jc.Item1);
            Assert.IsEmpty(staff);
        }

        [Test]
        public void GetEmployeeByUsername_ValidUsername_ReturnEmployee()
        {
            //empX = (3, "am", "ls", "Systems Administrator", "IT Building", "IT", 2, "a");
            var emp = _empObj.GetEmployeeByADUserName(empX.Item2);
            var staff = emp.PrimaryStaff.FirstOrDefault(s => s.Username == "al");

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
        public void GetEmployeeByUsername_Invalid_ArgumentException()
        {
            Assert.That(
                    () => _empObj.GetEmployeeById(_invalid.Item1)
                    ,
                    Throws
                    .TypeOf<ArgumentException>()
                    .With
                    .Property("Message")
                    .Matches(empErrorMessage)
                );
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
        public void GetEmployeePhone_InvalidId_NullReferenceException()
        {
            Assert.That(
                    () => _empObj.GetEmployeePhone(_invalid.Item1)
                    ,
                    Throws
                    .TypeOf<NullReferenceException>()
                    .With
                    .Property("Message")
                    .Matches(empErrorMessage)
                );
        }
    }
}
