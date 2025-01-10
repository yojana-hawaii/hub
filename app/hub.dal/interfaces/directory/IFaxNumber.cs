using hub.domain.model.directory;
using System.Collections.Generic;

namespace hub.dal.interfaces.directory
{
    public interface IFaxNumber
    {
        IEnumerable<FaxNumber> GetAllFaxNumbers();
        FaxNumber GetFaxNumberById(int faxId);
        IEnumerable<FaxNumber> GetFaxNumberByKeywordSearch(string searchKeyword);
        string GetFaxDepartment(int faxId);
        string GetFaxLocation(int faxId);
    }
}
