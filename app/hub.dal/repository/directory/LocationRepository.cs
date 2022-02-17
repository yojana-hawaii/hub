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
        private readonly string _locException = "Location not found";
        public LocationRepository(HubDbContext context)
        {
            _context = context;
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
                throw new ArgumentException(_locException, locId.ToString());
            return loc;
        }

        public IEnumerable<Employee> GetEmployeesByLocation(int locId)
        {
            try
            {

                return GetByLocationId(locId)
                    .Employees;
            }
            catch
            {
                throw new NullReferenceException(_locException);
            }
        }

        public int GetLocationId(string locName)
        {
            var loc = _context.Locations
                .FirstOrDefault(l => l.LocationName == locName);

            if (loc is null)
                throw new ArgumentException(_locException, locName);
            return loc.LocationId;
        }
    }
}
