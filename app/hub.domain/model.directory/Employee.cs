using System;
using System.Collections.Generic;

namespace hub.domain.model.directory
{
    public class Employee
    {
        //instantiate collection of objects referenced by this class
        public Employee()
        {
            //JobTitle = new JobTitle();
            //Department = new Department();
            //PrimaryManager = new Employee();
            PrimaryStaff = new List<Employee>();
            //Response_Question = new List<Response_Question>();
        }
        public int EmployeeId { get; set; }

        // from AD - can't modify
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime AccountCreated { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Name => $"{LastName}, {FirstName}";

        //from AD but modify from webapp
        public string Extension { get; set; }
        public string FullNumber { get; set; }

        //for search
        public string Keyword { get; set; }

        //in the future -DateTime not nullable from fluent API
        public DateTime? HireDate { get; set; }
        public string NickName { get; set; }
        public string EmployeeNumber { get; set; }
        public string PhotoPath { get; set; }

        //Foreign key
        public int? JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        //Self-Referencing foreign key
        public int? PrimaryManagerId { get; set; }
        public Employee PrimaryManager { get; set; }




        //Two-way foreign key loop
        public IEnumerable<Employee> PrimaryStaff { get; set; }
        //public IEnumerable<Response_Question> Response_Question { get; set; }
    }
}
