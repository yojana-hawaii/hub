using hub.dal.interfaces.directory;
using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace hub.dal.repository.directory
{
    public class DepartmentRepository : IDepartment
    {
        private readonly HubDbContext _context;
        
        private readonly Department _nullDept;
        private readonly List<Employee> _nullDeptEmployees;
        private readonly Employee _nullEmp;


        public DepartmentRepository(HubDbContext context)
        {
            _context = context;

            //null object design pattern.
            _nullEmp = new Employee()
            {
                EmployeeId = -1,
                Username = ""
            };
            _nullDeptEmployees = new List<Employee>
            {
                _nullEmp
            };
            _nullDept = new Department()
            {
                DepartmentId = -1,
                DepartmentName = "",
                Employees = _nullDeptEmployees
            };
        }
        private Department GetNullDepartment()
        {
            return _nullDept;
        }


        public IEnumerable<Department> GetAllDepartments()
        {
            return _context.Departments;
        }

        public Department GetByDepartmentId(int deptId)
        {
            var dept = _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefault(d => d.DepartmentId == deptId);
            if (dept is null)
                dept = GetNullDepartment();

            return dept;
        }

        public IEnumerable<Employee> GetEmployeesByDepartment(int deptId)
        {
            var emp = GetByDepartmentId(deptId).Employees;

            //emp is instanstiated in model class so it will never be null but it can be 0 employee
            //if (emp is null) emp = GetNullDepartment().Employees;
            if (emp.Count() == 0) emp = GetNullDepartment().Employees;

            return emp;
        }

        public int GetDepartmentId(string deptName)
        {
            var dept = _context.Departments
                .FirstOrDefault(d => d.DepartmentName == deptName);

            if (dept is null)
                dept = GetNullDepartment(); ;

            return dept.DepartmentId;
        }
    }
}
