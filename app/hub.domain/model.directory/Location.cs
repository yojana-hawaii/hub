using System.Collections.Generic;

namespace hub.domain.model.directory
{
    public class Location
    {
        public Location()
        {
            Employees = new List<Employee>();
        }
        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public IEnumerable<Employee> Employees { get; set; }
    }
}