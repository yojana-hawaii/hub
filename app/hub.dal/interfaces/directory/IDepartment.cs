using hub.domain.model.directory;
using System.Collections.Generic;

namespace hub.dal.interfaces.directory
{
    public interface IDepartment
    {
        IEnumerable<Department> GetAllDepartments();
        Department GetByDepartmentId(int deptId);
        IEnumerable<Employee> GetEmployeesByDepartment(int deptId);
        int GetDepartmentId(string deptName);
    }
}
