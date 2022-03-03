using hub.dal.interfaces.directory;
using hub.dal.shared;
using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
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
                emp = GetNullEmployee();

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

            if (_fullName is null) _fullName = "";
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
            var dept = GetEmployeeById(empId).Department;
            if (dept is null) dept = GetNullEmployee().Department;
            return dept.DepartmentName;
        }
        public string GetEmployeeJobTitle(int empId)
        {
            var job = GetEmployeeById(empId).JobTitle;
            if (job is null) job = GetNullEmployee().JobTitle;
            return job.JobTitleName;
        }
        public string GetEmployeeLocation(int empId)
        {
            var loc = GetEmployeeById(empId).Location;
            if (loc is null) loc = GetNullEmployee().Location;
            return loc.LocationName;
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
            IEnumerable<Employee> allEmployees = GetAllEmployees();
            IEnumerable<Employee> selectEmployees = allEmployees;

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                
                /* call static method in static class without creating new object
                clean up user input
                 -> remove whitespace in the beginging and the end
                 -> remove special characters
                */
                searchKeyword = UserInputValidation.GetUserInputWithoutSpecialCharacterAndWhitespaces(searchKeyword);

                //if two space in a row in user input -> split takes one space as part if input -> space always returns entire all employees
                //Where after split removes those empty string after split and convert to list.
                List<string> searchList = searchKeyword.Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).ToList();

                selectEmployees = allEmployees
                                .Where(
                                    e => (e.LastName != null && searchList.Any(term => e.LastName.ToLower().Contains(term)))
                                        || (e.FirstName != null && searchList.Any(term => e.FirstName.ToLower().Contains(term)))
                                        || (e.Extension != null && searchList.Any(term => e.Extension.ToLower().Contains(term)))
                                        || (e.FullNumber != null && searchList.Any(term => e.FullNumber.ToLower().Contains(term)))
                                        || (e.Email != null && searchList.Any(term => e.Email.ToLower().Contains(term)))
                                        || (e.JobTitle != null && searchList.Any(term => e.JobTitle.JobTitleName.ToLower().Contains(term)))
                                        || (e.Department != null && searchList.Any(term => e.Department.DepartmentName.ToLower().Contains(term)))
                                        || (e.Location != null && searchList.Any(term => e.Location.LocationName.ToLower().Contains(term)))
                                );

            }
            //if list is empty, nothing returns so if employee count == 0 return all employee -> can remove this as 
            if (selectEmployees.Count() == 0) selectEmployees = allEmployees;


            return selectEmployees;

        }

    }
}
