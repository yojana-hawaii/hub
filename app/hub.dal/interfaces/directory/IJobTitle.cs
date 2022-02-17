using hub.domain.model.directory;
using System.Collections.Generic;

namespace hub.dal.interfaces.directory
{
    public interface IJobTitle
    {
        IEnumerable<JobTitle> GetAllJobTitles();
        JobTitle GetByJobTitleId(int jobId);
        IEnumerable<Employee> GetEmployeesByJobTitle(int jobId);
        int GetJobTitleId(string titleName);
    }
}
