using hub.dal.interfaces.directory;
using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hub.dal.repository.directory
{
    public class EmployeeRepository : IEmployee
    {
        private readonly HubDbContext _context;
        private readonly string _employeeExceptionMessage = "Employee not found";


        public EmployeeRepository(HubDbContext appDbContext)
        {
            _context = appDbContext;
        }



        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Location);
            //.Where(emp => !emp.Keyword.Contains("*"));
        }

        public IEnumerable<Employee> GetAllNonEmployees()
        {
            return _context.Employees
                .Where(emp => emp.Keyword.Contains("*"));
        }

        public Employee GetEmployeeById(int empId)
        {
            var emp = GetAllEmployees()
                .FirstOrDefault(e => e.EmployeeId == empId);

            if (emp is null)
                throw new ArgumentException(_employeeExceptionMessage, empId.ToString());

            return emp;
        }

        public string GetFullName(int empId)
        {
            try
            {
                var emp = GetEmployeeById(empId);
                var _fullName = emp.LastName;

                if (!string.IsNullOrWhiteSpace(_fullName))
                {
                    if (!string.IsNullOrWhiteSpace(emp.FirstName))
                    {
                        _fullName += ", ";
                    }
                    _fullName += emp.FirstName;
                }
                else
                {
                    _fullName = emp.FirstName;
                }
                return _fullName;
            }
            catch
            {
                throw new NullReferenceException(_employeeExceptionMessage);
            }

        }

        public string GetEmployeePhone(int employeeId)
        {
            try
            {
                var emp = GetEmployeeById(employeeId);
                var _phone = emp.Extension;

                if (!string.IsNullOrWhiteSpace(_phone))
                {
                    if (!string.IsNullOrWhiteSpace(emp.FullNumber))
                    {
                        _phone += " -> ";
                    }
                    _phone += emp.FullNumber;
                }
                else
                {
                    _phone = emp.FullNumber;
                }
                return _phone;
            }
            catch
            {
                throw new NullReferenceException(_employeeExceptionMessage);
            }
        }



        public string GetEmployeeDepartment(int empId)
        {
            string _dept;
            try
            {
                _dept = GetEmployeeById(empId).Department.DepartmentName;
            }
            catch (Exception)
            {
                _dept = "";
            }
            return _dept;
        }
        public string GetEmployeeJobTitle(int empId)
        {
            string _job;
            try
            {
                _job = GetEmployeeById(empId).JobTitle.JobTitleName;
            }
            catch (Exception)
            {
                _job = "";
            }
            return _job;
        }
        public string GetEmployeeLocation(int empId)
        {
            string _loc;
            try
            {
                _loc = GetEmployeeById(empId).Location.LocationName;
            }
            catch (Exception)
            {
                _loc = "";
            }
            return _loc;
        }

        public Employee GetManager(int empId)
        {
            try
            {
                return GetEmployeeById(empId).PrimaryManager;
            }
            catch
            {
                throw new NullReferenceException(_employeeExceptionMessage);
            }
        }

        public IEnumerable<Employee> GetStaffs(int managerId)
        {
            try
            {
                return GetEmployeeById(managerId).PrimaryStaff;
            }
            catch
            {
                throw new NullReferenceException(_employeeExceptionMessage);
            }
        }

        public Employee GetEmployeeByADUserName(string username)
        {
            var emp = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Location)
                .Include(e => e.PrimaryManager)
                .Include(e => e.PrimaryStaff)
                .FirstOrDefault(e => e.Username == username);

            if (emp is null)
                throw new ArgumentException(_employeeExceptionMessage, username.ToString());

            return emp;
        }

        public IEnumerable<Employee> GetEmployeeByKeywordSearch(string searchKeyword)
        {

            IEnumerable<Employee> matchingEmployees = new List<Employee>();
            if (!string.IsNullOrEmpty(searchKeyword))
            {
                string[] newSearch = searchKeyword.Split(' ');
                IEnumerable<Employee> employees = GetAllEmployees(); ;

                foreach (var str in newSearch)
                {
                    matchingEmployees = employees.Where(emp => emp.Keyword.ToLower().Contains(str.ToLower()));
                }
            }


            return matchingEmployees;

        }

    }
}
