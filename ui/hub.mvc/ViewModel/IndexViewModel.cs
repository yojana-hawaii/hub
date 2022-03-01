using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hub.mvc.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<EmployeeViewModel> EmployeeViewModels { get; set; }
        public string SearchKeyword { get; set; }
    }
}
