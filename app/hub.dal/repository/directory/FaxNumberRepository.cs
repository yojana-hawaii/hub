using hub.dal.interfaces.directory;
using hub.dal.shared;
using hub.dbMigration.dbContext;
using hub.domain.model.directory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace hub.dal.repository.directory
{
    public class FaxNumberRepository : IFaxNumber
    {
        private readonly HubDbContext _context;
        private readonly FaxNumber _nullFaxNumber;
        private readonly Department _nullDepartment;
        private readonly Location _nullLocation;
        public FaxNumberRepository(HubDbContext context)
        {
            _context = context;

            _nullDepartment = new Department()
            {
                DepartmentId = -1,
                DepartmentName = ""
            };
            _nullLocation = new Location()
            {
                LocationId = -1,
                LocationName = ""
            };
            _nullFaxNumber = new FaxNumber()
            {
                FaxId = -1,
                FaxName = "",
                Department = _nullDepartment,
                Location = _nullLocation,
                Number = ""
            };
        }

        private FaxNumber GetNullFaxNumber()
        {
            return _nullFaxNumber;
        }

        public IEnumerable<FaxNumber> GetAllFaxNumbers()
        {
            return _context.FaxNumbers
                .Include(f => f.Department)
                .Include(f => f.Location);
        }
        public FaxNumber GetFaxNumberById(int faxId)
        {
            var fax = GetAllFaxNumbers()
                .FirstOrDefault(f => f.FaxId == faxId);
            if (fax is null)
                fax = GetNullFaxNumber();
            return fax;
        }

        public string GetFaxDepartment(int faxId)
        {
            var dept = GetFaxNumberById(faxId).Department;
            if (dept is null)
                dept = GetNullFaxNumber().Department;
            return dept.DepartmentName;
        }

        public string GetFaxLocation(int faxId)
        {
            var loc = GetFaxNumberById(faxId).Location;
            if (loc is null)
                loc = GetNullFaxNumber().Location;
            return loc.LocationName;
        }

        public IEnumerable<FaxNumber> GetFaxNumberByKeywordSearch(string searchKeyword)
        {
            var allFaxes = GetAllFaxNumbers();
            var selectFaxes = allFaxes;

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                searchKeyword = UserInputValidation.GetUserInputWithoutSpecialCharacterAndWhitespaces(searchKeyword);
                List<string> searchList = searchKeyword.Split(' ').Where(str => !string.IsNullOrWhiteSpace(str)).ToList();

                selectFaxes = allFaxes
                    .Where(
                        f => (f.Department != null && searchList.Any(term => f.Department.DepartmentName.ToLower().Contains(term)))
                            || (f.Location != null && searchList.Any(term => f.Location.LocationName.ToLower().Contains(term)))
                            || (f.FaxName != null && searchList.Any(term => f.FaxName.ToLower().Contains(term)))
                            || (f.Number != null && searchList.Any(term => f.Number.ToLower().Contains(term)))
                    );

            }
            else
            {
                selectFaxes = new List<FaxNumber>()
                {
                    GetNullFaxNumber()
                };
            }

            if (selectFaxes.Count() == 0) selectFaxes = allFaxes;
            return selectFaxes;
        }




    }
}
