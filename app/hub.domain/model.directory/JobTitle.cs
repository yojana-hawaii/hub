using System.Collections.Generic;

namespace hub.domain.model.directory
{
    public class JobTitle
    {
        public JobTitle()
        {
            Employees = new List<Employee>();
        }

        public int JobTitleId { get; set; }
        public string JobTitleName { get; set; }


        //Two-way foreign key loop
        public IEnumerable<Employee> Employees { get; set; }
    }
}