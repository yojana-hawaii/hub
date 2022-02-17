using hub.domain.model.directory;
using System.Collections.Generic;

namespace hub.dal.interfaces.directory
{
    public interface ILocation
    {
        IEnumerable<Location> GetAllLocations();
        Location GetByLocationId(int locId);
        IEnumerable<Employee> GetEmployeesByLocation(int locId);
        int GetLocationId(string locName);
    }
}
