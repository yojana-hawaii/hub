using hub.dal.interfaces.directory;
using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hub.dal.repository.directory
{
    public class JobTitleRepository : IJobTitle
    {
        private readonly HubDbContext _context;

        private readonly JobTitle _nullJob;
        private readonly List<Employee> _nullJobEmployees;
        private readonly Employee _nullEmp;

        public JobTitleRepository(HubDbContext context)
        {
            _context = context;

            //null object design pattern.
            _nullEmp = new Employee()
            {
                EmployeeId = -1,
                Username = ""
            };
            _nullJobEmployees = new List<Employee>
            {
                _nullEmp
            };
            _nullJob = new JobTitle()
            {
                JobTitleId = -1,
                JobTitleName = "",
                Employees = _nullJobEmployees
            };
        }

        private JobTitle GetNullJobTitle()
        {
            return _nullJob;
        }


        public IEnumerable<JobTitle> GetAllJobTitles()
        {
            return _context.JobTitles;
        }

        public JobTitle GetByJobTitleId(int jobId)
        {
            var job = _context.JobTitles
                .Include(j => j.Employees)
                .FirstOrDefault(j => j.JobTitleId == jobId);

            if (job is null)
                job = GetNullJobTitle();
            return job;
        }

        public IEnumerable<Employee> GetEmployeesByJobTitle(int jobId)
        {
            var emp = GetByJobTitleId(jobId).Employees;

            //emp is instanstiated in model class so it will never be null but it can be 0 employee
            //if (emp is null) emp = GetNullJobTitle().Employees;
            if (emp.Count() == 0) emp = GetNullJobTitle().Employees;

            return emp;
        }

        public int GetJobTitleId(string titleName)
        {
            var job = _context.JobTitles
                .FirstOrDefault(j => j.JobTitleName == titleName);

            if (job is null)
                job = GetNullJobTitle();

            return job.JobTitleId;
        }
    }
}
