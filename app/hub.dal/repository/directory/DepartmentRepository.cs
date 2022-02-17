using hub.dal.interfaces.directory;
using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hub.dal.repository.directory
{
    public class DepartmentRepository : IDepartment
    {
        private readonly HubDbContext _context;
        private readonly string _departmentExceptionMessage = "Department not found";

        public DepartmentRepository(HubDbContext context)
        {
            _context = context;
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
                throw new ArgumentException(_departmentExceptionMessage, deptId.ToString());

            return dept;
        }

        public IEnumerable<Employee> GetDepartmentEmployees(int deptId)
        {
            try
            {

                return GetByDepartmentId(deptId)
                    .Employees;
            }
            catch (ArgumentException)
            {
                throw new NullReferenceException(_departmentExceptionMessage);
            }
        }

        public int GetDepartmentId(string deptName)
        {
            var dept = _context.Departments
                .FirstOrDefault(d => d.DepartmentName == deptName);

            if (dept is null)
                throw new ArgumentException(_departmentExceptionMessage, deptName);

            return dept.DepartmentId;
        }
    }
}
