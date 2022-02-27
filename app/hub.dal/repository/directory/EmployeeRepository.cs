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

        private readonly Employee _nullEmp;
        private readonly Employee _nullManager;
        private readonly List<Employee> _nullStaff;
        private readonly Location _nullLoc;
        private readonly JobTitle _nullJob;
        private readonly Department _nullDept;


        public EmployeeRepository(HubDbContext appDbContext)
        {
            _context = appDbContext;

            //null object design pattern
            _nullLoc = new Location()
            {
                LocationId = -1,
                LocationName = ""
            };
            _nullJob = new JobTitle()
            {
                JobTitleId = -1,
                JobTitleName = ""
            };
            _nullDept = new Department()
            {
                DepartmentId = -1,
                DepartmentName = ""
            };
            _nullManager = new Employee()
            {
                EmployeeId = -1,
                Username = ""
            };
            _nullStaff = new List<Employee>()
            {
                _nullManager
            };
            _nullEmp = new Employee()
            {
                EmployeeId = -1,
                Username = "",
                FirstName = "",
                LastName = "",
                Email = "",
                Department = _nullDept,
                JobTitle = _nullJob,
                Location = _nullLoc,
                PrimaryManager = _nullManager,
                PrimaryStaff = _nullStaff
            };
        }


        private Employee GetNullEmployee()
        {
            return _nullEmp;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Location);
        }


        public Employee GetEmployeeById(int empId)
        {
            var emp = GetAllEmployees()
                .FirstOrDefault(e => e.EmployeeId == empId);

            if (emp is null)
                return GetNullEmployee();

            return emp;
        }

        public string GetFullName(int empId)
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

        public string GetEmployeePhone(int employeeId)
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
            if (_phone is null)
                _phone = "";
            return _phone;
        }



        public string GetEmployeeDepartment(int empId)
        {
            return GetEmployeeById(empId).Department.DepartmentName;
        }
        public string GetEmployeeJobTitle(int empId)
        {
            return GetEmployeeById(empId).JobTitle.JobTitleName;
        }
        public string GetEmployeeLocation(int empId)
        {
            return GetEmployeeById(empId).Location.LocationName;
        }

        public Employee GetManager(int empId)
        {
            var mngr = GetEmployeeById(empId).PrimaryManager;
            if (mngr is null)
                mngr = GetNullEmployee();

            return mngr;
        }

        public IEnumerable<Employee> GetStaffs(int managerId)
        {
            var staff = GetEmployeeById(managerId).PrimaryStaff;
            if (staff is null)
                staff = new List<Employee>() { GetNullEmployee() };

            return staff;
        }

        public Employee GetEmployeeByADUserName(string username)
        {
            var emp = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.JobTitle)
                .Include(e => e.Location)
                .FirstOrDefault(e => e.Username == username);

            if (emp is null)
                emp = _nullEmp;

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
                    //matchingEmployees = employees.Where(emp => emp.Keyword.ToLower().Contains(str.ToLower()));
                    matchingEmployees = employees
                        .Where(
                        e => (e.LastName != null && e.LastName.Contains(str))
                            || (e.FirstName != null && e.FirstName.Contains(str))
                            || (e.Extension != null && e.Extension.Contains(str))
                            || (e.FullNumber != null && e.FullNumber.Contains(str))
                            || (e.Email != null && e.Email.Contains(str))
                            || (e.JobTitle != null && e.JobTitle.JobTitleName.Contains(str))
                            || (e.Department != null && e.Department.DepartmentName.Contains(str))
                            || (e.Location != null && e.Location.LocationName.Contains(str))
                        );
                }
            }


            return matchingEmployees;

        }

    }
}
