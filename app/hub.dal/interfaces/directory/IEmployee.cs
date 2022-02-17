using hub.domain.model.directory;
using System;
using System.Collections.Generic;
using System.Text;

namespace hub.dal.interfaces.directory
{
    public interface IEmployee
    {
        IEnumerable<Employee> GetAllEmployees();
        IEnumerable<Employee> GetAllNonEmployees();

        Employee GetEmployeeById(int empId);
        Employee GetEmployeeByADUserName(string username);
        IEnumerable<Employee> GetEmployeeByKeywordSearch(string searchKeyword);
        string GetEmployeeDepartment(int empId);
        string GetEmployeeJobTitle(int empId);
        string GetEmployeeLocation(int empId);
        Employee GetManager(int empId);
        IEnumerable<Employee> GetStaffs(int managerId);

        string GetFullName(int empId);
        string GetEmployeePhone(int employeeId);
    }
}
