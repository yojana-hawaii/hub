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
        private readonly string _jobTitleExceptionMessage = "Job Title not found";

        public JobTitleRepository(HubDbContext context)
        {
            _context = context;
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
            {
                throw new ArgumentException(_jobTitleExceptionMessage, jobId.ToString());
            }
            return job;
        }

        public IEnumerable<Employee> GetEmployeesByJobTitle(int jobId)
        {
            try
            {

                return GetByJobTitleId(jobId)
                    .Employees;
            }
            catch (ArgumentException)
            {
                throw new NullReferenceException(_jobTitleExceptionMessage);
            }
        }

        public int GetJobTitleId(string titleName)
        {
            var job = _context.JobTitles
                .Include(j => j.Employees)
                .FirstOrDefault(j => j.JobTitleName == titleName);

            if (job is null)
                throw new ArgumentException(_jobTitleExceptionMessage, titleName);
            return job.JobTitleId;
        }
    }
}
