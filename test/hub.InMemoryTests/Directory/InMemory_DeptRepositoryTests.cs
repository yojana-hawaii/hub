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
    class InMemory_DeptRepositoryTests : HubDbContextBase
    {

        private DepartmentRepository _deptObj;
        private (int, string) itDept;
        private (int, string) acctDept;
        private (int, string) billDept;
        private (int, string) execDept;
        private (int, string) invalidDept;
        private int deptCount;
        private int itCount;
        private (int, string) user;
        private string deptErrorMessage;

        [SetUp]
        public void Setup()
        {
            _deptObj = new DepartmentRepository(_context);
            itDept = (1, "IT");
            acctDept = (2, "Accounting");
            billDept = (3, "Billing");
            execDept = (4, "Exec");
            invalidDept = (99, "Marketing");
            deptCount = 4;
            deptErrorMessage = "Department not found";
            itCount = 5;
            user = (10, "amm");
        }

        //Valid Tests
        [Test]
        public void GetAllDept_Valid_ReturnDeptList()
        {
            IEnumerable<Department> depts = _deptObj.GetAllDepartments();

            Assert.IsNotNull(depts);
            CollectionAssert.AllItemsAreUnique(depts);
            CollectionAssert.AllItemsAreNotNull(depts);

            Assert.That(depts.Count(d => d.DepartmentName == itDept.Item2), Is.EqualTo(1));
            Assert.That(depts.Count(d => d.DepartmentName == acctDept.Item2), Is.EqualTo(1));
            Assert.That(depts.Count(d => d.DepartmentName == billDept.Item2), Is.EqualTo(1));
            Assert.That(depts.Count(d => d.DepartmentName == execDept.Item2), Is.EqualTo(1));

            Assert.That(deptCount, Is.EqualTo(depts.Count()));
        }
        [Test]
        public void GetDeptById_ValidId_ReturnDept()
        {
            Department it = _deptObj.GetByDepartmentId(itDept.Item1);
            Department bill = _deptObj.GetByDepartmentId(billDept.Item1);

            Assert.AreEqual(itDept.Item2, it.DepartmentName);
            Assert.AreEqual(billDept.Item2, bill.DepartmentName);
        }
        [Test]
        public void GetEmplyeesByDeptId_ValidId_ReturnEmployees()
        {
            IEnumerable<Employee> emps = _deptObj.GetDepartmentEmployees(itDept.Item1);

            Assert.IsNotNull(emps);
            Assert.That(itCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            Assert.That(emps.Count(e => e.Username == user.Item2), Is.EqualTo(1));
        }
        [Test]
        public void GetDeptId_ValidName_ReturnDeptId()
        {
            int deptId = _deptObj.GetDepartmentId(execDept.Item2);
            Assert.AreEqual(execDept.Item1, deptId);
        }

        //Invalid Tests
        [Test]
        public void GetDeptById_InvalidId_ArgumentException()
        {
            Assert.That(
                () => _deptObj.GetByDepartmentId(invalidDept.Item1)
                ,
                Throws
                .TypeOf<ArgumentException>()
                .With
                .Property("Message")
                .Matches(deptErrorMessage)
                );
        }
        [Test]
        public void GetEmployeesByDeptId_InValidId_NullReferenceException()
        {
            Assert.That(
                    () => _deptObj.GetDepartmentEmployees(invalidDept.Item1)
                    ,
                    Throws
                    .TypeOf<NullReferenceException>()
                    .With
                    .Property("Message")
                    .Matches(deptErrorMessage)
                );
        }
        [Test]
        public void GetDeptId_InvalidName_ArgumentException()
        {
            Assert.That(
                    () => _deptObj.GetDepartmentId(invalidDept.Item2)
                    ,
                    Throws
                    .TypeOf<ArgumentException>()
                    .With
                    .Property("Message")
                    .Matches(deptErrorMessage)
                );
        }
    }
}

