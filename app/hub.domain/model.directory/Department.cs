using System.Collections.Generic;

namespace hub.domain.model.directory
{
    public class Department
    {
        public Department()
        {
            Employees = new List<Employee>();
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public IEnumerable<Employee> Employees { get; set; }
    }
}