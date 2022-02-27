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
    class InMemory_JobTitleReposityTests : HubDbContextBase
    {
        private JobTitleRepository _jobObj;
        private (int, string) sysAdmin;
        private (int, string) payroll;
        private (int, string) cfo;
        private (int, string) ceo;
        private (int, string) _invalid;
        private (int, string) newJob;
        private int jobTitleCount;
        private int sysadminCount;
        private (int, string) user;


        [SetUp]
        public void Setup()
        {
            _jobObj = new JobTitleRepository(_context);
            sysAdmin = (1, "Systems Administrator");
            payroll = (2, "Payroll");
            cfo = (3, "CFO");
            ceo = (4, "CEO");
            _invalid = (-1, "");
            newJob = (-1, "");
            jobTitleCount = 5;
            sysadminCount = 5;
            user = (10, "amm");
        }


        [Test]
        public void GetAllJobTitles_Valid_ReturnJobList()
        {
            IEnumerable<JobTitle> jobs = _jobObj.GetAllJobTitles();

            Assert.IsNotNull(jobs);
            Assert.That(jobTitleCount, Is.EqualTo(jobs.Count()));
            CollectionAssert.AllItemsAreUnique(jobs);
            CollectionAssert.AllItemsAreNotNull(jobs);

            Assert.That(jobs.Count(d => d.JobTitleName == sysAdmin.Item2), Is.EqualTo(1));

            var oneJob = jobs.FirstOrDefault(j => j.JobTitleId == sysAdmin.Item1);
            Assert.That(oneJob.JobTitleName, Is.EqualTo(sysAdmin.Item2));
        }

        [Test]
        public void GetByJobTitleId_ValidId_ReturnOneJob()
        {
            JobTitle job1 = _jobObj.GetByJobTitleId(ceo.Item1);
            JobTitle job2 = _jobObj.GetByJobTitleId(payroll.Item1);

            Assert.IsNotNull(job1);
            Assert.IsNotNull(job2);

            Assert.AreEqual(ceo.Item2, job1.JobTitleName);
            Assert.AreEqual(payroll.Item2, job2.JobTitleName);
        }
        [Test]
        public void GetByJobTitleId_InvalidId_ReturnOneNullJob()
        {
            var job = _jobObj.GetByJobTitleId(_invalid.Item1);

            Assert.IsNotNull(job);

            Assert.AreEqual(_invalid.Item2, job.JobTitleName);
        }


        [Test]
        public void GetEmployeesByJobTitle_ValidId_ReturnEmployeesList()
        {
            IEnumerable<Employee> emps = _jobObj.GetEmployeesByJobTitle(sysAdmin.Item1);

            Assert.IsNotNull(emps);
            Assert.That(sysadminCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);

            Assert.That(emps.Count(e => e.Username == user.Item2), Is.EqualTo(1));

            var oneEmp = emps.FirstOrDefault(e => e.EmployeeId == user.Item1);
            Assert.That(oneEmp.Username, Is.EqualTo(user.Item2));
        }
        [Test]
        public void GetEmployeesByJobTitle_InvalidId_ReturnNullEmployeeList()
        {
            var emps = _jobObj.GetEmployeesByJobTitle(_invalid.Item1);

            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            var nullEmp = emps.FirstOrDefault(e => e.EmployeeId == _invalid.Item1);
            Assert.That(nullEmp.EmployeeId, Is.EqualTo(_invalid.Item1));
            Assert.That(nullEmp.Username, Is.EqualTo(_invalid.Item2));
        }
        [Test]
        public void GetEmployeeByJobJobTitle_MisingEmployee_ReturnNullEmployeeList()
        {
            var emps = _jobObj.GetEmployeesByJobTitle(newJob.Item1);

            Assert.IsNotNull(emps);
            Assert.That(1, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreUnique(emps);
            CollectionAssert.AllItemsAreNotNull(emps);

            var nullEmp = emps.FirstOrDefault(e => e.EmployeeId == _invalid.Item1);
            Assert.That(nullEmp.Username, Is.EqualTo(_invalid.Item2));
        }

        [Test]
        public void GetJobTitleId_ValidName_ReturnOneJobId()
        {
            int jobid = _jobObj.GetJobTitleId(cfo.Item2);
            Assert.AreEqual(cfo.Item1, jobid);
        }
        [Test]
        public void GetJobTitleId_InvalidName_ReturnsOneNullJobId()
        {
            var jobid = _jobObj.GetJobTitleId(_invalid.Item2);
            Assert.AreEqual(_invalid.Item1, jobid);
        }
    }
}
