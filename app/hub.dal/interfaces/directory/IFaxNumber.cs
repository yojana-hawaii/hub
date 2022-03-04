using hub.domain.model.directory;
using System.Collections.Generic;

namespace hub.dal.interfaces.directory
{
    public interface IFaxNumber
    {
        IEnumerable<FaxNumber> GetAllFaxNumbers();
        IEnumerable<FaxNumber> GetFaxNumberByKeywordSearch(string searchKeyword);
    }
}
