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
        private (int, string) invalid;
        private int jobTitleCount;
        private int sysadminCount;
        private (int, string) user;
        private string jobErrorMessage;


        [SetUp]
        public void Setup()
        {
            _jobObj = new JobTitleRepository(_context);
            sysAdmin = (1, "Systems Administrator");
            payroll = (2, "Payroll");
            cfo = (3, "CFO");
            ceo = (4, "CEO");
            invalid = (99, "Marketing Director");
            jobTitleCount = 4;
            sysadminCount = 5;
            user = (10, "amm");
            jobErrorMessage = "Job Title not found";
        }


        [Test]
        public void GetAllJobTitles_Valid_ReturnListOfDept()
        {
            IEnumerable<JobTitle> jobs = _jobObj.GetAllJobTitles();

            Assert.IsNotNull(jobs);
            Assert.That(jobTitleCount, Is.EqualTo(jobs.Count()));
            CollectionAssert.AllItemsAreUnique(jobs);
            CollectionAssert.AllItemsAreNotNull(jobs);
            Assert.That(jobs.Count(d => d.JobTitleName == sysAdmin.Item2), Is.EqualTo(1));
        }

        [Test]
        public void GetJobById_ValidId_ReturnDeptObject()
        {
            JobTitle job1 = _jobObj.GetByJobTitleId(ceo.Item1);
            JobTitle job2 = _jobObj.GetByJobTitleId(payroll.Item1);

            Assert.AreEqual(ceo.Item2, job1.JobTitleName);
            Assert.AreEqual(payroll.Item2, job2.JobTitleName);
        }
        [Test]
        public void GetJobById_InvalidId_ArgumentException()
        {
            Assert.That(
                    () => _jobObj.GetByJobTitleId(invalid.Item1)
                    ,
                    Throws
                    .TypeOf<ArgumentException>()
                    .With
                    .Property("Message")
                    .Matches(jobErrorMessage)
                );
        }

        [Test]
        public void GetEmployeeByJobId_ValidId_ReturnEmployees()
        {
            IEnumerable<Employee> emps = _jobObj.GetEmployeesByJobTitle(sysAdmin.Item1);

            Assert.IsNotNull(emps);
            Assert.That(sysadminCount, Is.EqualTo(emps.Count()));
            CollectionAssert.AllItemsAreNotNull(emps);
            CollectionAssert.AllItemsAreUnique(emps);
            Assert.That(emps.Count(e => e.Username == user.Item2), Is.EqualTo(1));
        }
        [Test]
        public void GetEmployeeById_InvalidId_NullReferenceException()
        {
            Assert.That(
                    () => _jobObj.GetEmployeesByJobTitle(invalid.Item1)
                    ,
                    Throws
                    .TypeOf<NullReferenceException>()
                    .With
                    .Property("Message")
                    .Matches(jobErrorMessage)
                );
        }

        [Test]
        public void GetJobId_ValidName_ReturnJobName()
        {
            int jobid = _jobObj.GetJobTitleId(cfo.Item2);
            Assert.AreEqual(cfo.Item1, jobid);
        }
        [Test]
        public void GetJobId_InvalidName_ArgumentException()
        {
            Assert.That(
                    () => _jobObj.GetJobTitleId(invalid.Item2)
                    ,
                    Throws
                    .TypeOf<ArgumentException>()
                    .With
                    .Property("Message")
                    .Matches(jobErrorMessage)
                );
        }
    }
}
