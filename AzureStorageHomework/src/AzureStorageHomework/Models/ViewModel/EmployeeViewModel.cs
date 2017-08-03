using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageHomework.Models.ViewModel
{
    public class EmployeeViewModel
    {
        public string Title { get; set; }
        public IEnumerable<EmployeeModel> Employees { get; set; }
    }
}
