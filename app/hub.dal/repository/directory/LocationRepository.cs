using hub.dal.interfaces.directory;
using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hub.dal.repository.directory
{
    public class LocationRepository : ILocation
    {
        private readonly HubDbContext _context;

        private readonly Location _nulllLoc;
        private readonly List<Employee> _nullLocEmployees;
        private readonly Employee _nullEmp;

        public LocationRepository(HubDbContext context)
        {
            _context = context;

            //null object design pattern.
            _nullEmp = new Employee()
            {
                EmployeeId = -1,
                Username = ""
            };
            _nullLocEmployees = new List<Employee>
            {
                _nullEmp
            };
            _nulllLoc = new Location()
            {
                LocationId = -1,
                LocationName = "",
                Employees = _nullLocEmployees
            };
        }

        private Location GetNullLocation()
        {
            return _nulllLoc;
        }


        public IEnumerable<Location> GetAllLocations()
        {
            return _context.Locations;
        }

        public Location GetByLocationId(int locId)
        {
            var loc = _context.Locations
                .Include(l => l.Employees)
                .FirstOrDefault(l => l.LocationId == locId);

            if (loc is null)
                loc = GetNullLocation();
            return loc;
        }

        public IEnumerable<Employee> GetEmployeesByLocation(int locId)
        {
            var emp = GetByLocationId(locId).Employees;

            //emp is instanstiated in model class so it will never be null but it can be 0 employee
            //if (emp is null) emp = GetNullLocation().Employees;
            if (emp.Count() == 0) emp = GetNullLocation().Employees;

            return emp;
        }

        public int GetLocationId(string locName)
        {
            var loc = _context.Locations
                .FirstOrDefault(l => l.LocationName == locName);

            if (loc is null)
                loc = GetNullLocation();

            return loc.LocationId;
        }
    }
}
