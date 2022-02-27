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
        private (int, string) _invalid;
        private (int, string) newDepartment;
        private int deptCount;
        private int itCount;
        private (int, string) user;

        [SetUp]
        public void Setup()
        {
            _deptObj = new DepartmentRepository(_context);
            itDept = (1, "IT");
            acctDept = (2, "Accounting");
            billDept = (3, "Billing");
            execDept = (4, "Exec");
            _invalid = (-1, "");
            newDepartment = (5, "");
            deptCount = 5;
            itCount = 5;
            user = (10, "amm");
        }


        [Test]
        public void GetAllDepartments_Valid_ReturnDeptList()
        {
            IEnumerable<Department> depts = _deptObj.GetAllDepartments();

            Assert.IsNotNull(depts);
            Assert.That(deptCount, Is.EqualTo(depts.Count()));
            CollectionAssert.AllItemsAreUnique(depts);
            CollectionAssert.AllItemsAreNotNull(depts);

            Assert.That(depts.Count(d => d.DepartmentName == itDept.Item2), Is.EqualTo(1));
            Assert.That(depts.Count(d => d.DepartmentName == acctDept.Item2), Is.EqualTo(1));
            Assert.That(depts.Count(d => d.DepartmentName == billDept.Item2), Is.EqualTo(1));
            Assert.That(depts.Count(d => d.DepartmentName == execDept.Item2), Is.EqualTo(1));


            var oneDept = depts.FirstOrDefault(d => d.DepartmentId == itDept.Item1);
            Assert.That(oneDept.DepartmentName, Is.EqualTo(itDept.Item2));
        }
        
        
        [Test]
        public void GetByDepartmentId_ValidId_ReturnOneDept()
        {
            Department it = _deptObj.GetByDepartmentId(itDept.Item1);
            Department bill = _deptObj.GetByDepartmentId(billDept.Item1);

            Assert.IsNotNull(it);
            Assert.IsNotNull(bill);

            Assert.AreEqual(itDept.Item2, it.DepartmentName);
            Assert.AreEqual(billDept.Item2, bill.DepartmentName);
        }
        [Test]
        public void GetByDepartmentId_InvalidId_ReturnOneNullDepartment()
        {
            var dept = _deptObj.GetByDepartmentId(_invalid.Item1);

            Assert.IsNotNull(dept);

            Assert.AreEqual(_invalid.Item2, dept.DepartmentName);
            Assert.AreEqual(_invalid.Item1, dept.DepartmentId);
        }


        [Test]
        public void GetEmployeesByDepartment_ValidId_ReturnEmployeeList()
        {
            IEnumerable<Employee> emps = _deptObj.GetEmployeesByDepartment(itDept.Item1);

            Assert.IsNotNull(emps);
            Assert.That(itCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            Assert.That(emps.Count(e => e.Username == user.Item2), Is.EqualTo(1));

            var oneEmp = emps.FirstOrDefault(e => e.EmployeeId == user.Item1);
            Assert.That(oneEmp.Username, Is.EqualTo(user.Item2));
        }
        [Test]
        public void GetEmployeesByDepartment_InValidId_ReurnsNullEmployeeList()
        {
            var emps = _deptObj.GetEmployeesByDepartment(_invalid.Item1);

            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            var nullEmp = emps.FirstOrDefault(e => e.EmployeeId == _invalid.Item1);
            Assert.That(nullEmp.EmployeeId, Is.EqualTo(_invalid.Item1));
            Assert.That(nullEmp.Username, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetEmployeesByDepartment_MisingEmployee_ReturnNullEmployeeList()
        {
            var emps = _deptObj.GetEmployeesByDepartment(newDepartment.Item1);

            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            var nullEmp = emps.FirstOrDefault(e => e.EmployeeId == _invalid.Item1);
            Assert.That(nullEmp.Username, Is.EqualTo(_invalid.Item2));
        }

        [Test]
        public void GetDepartmentId_ValidName_ReturnOneDeptId()
        {
            int deptId = _deptObj.GetDepartmentId(execDept.Item2);
            Assert.AreEqual(execDept.Item1, deptId);
        }       
        [Test]
        public void GetDepartmentId_InvalidName_ReturnOneNullDepartmentId()
        {
            var deptId = _deptObj.GetDepartmentId(_invalid.Item2);
            Assert.AreEqual(_invalid.Item1, deptId);
        }
    }
}

