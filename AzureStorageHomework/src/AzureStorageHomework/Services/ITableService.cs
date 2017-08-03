using AzureStorageHomework.Models;
using AzureStorageHomework.Models.Entities;
using AzureStorageHomework.Models.ViewModel;
using System.Threading.Tasks;

namespace AzureStorageHomework.Services
{
    public interface ITableService
    {
        Task<EmployeeViewModel> GetEmployeeList();
        Task<EmployeeModel> GetEmployeeDetails(string id);
        Task<bool> CreateEntry(EmployeeModel employee);
        Task<bool> UpdateEntry(EmployeeModel employee);
        Task<bool> DeleteEntry(string id);
    }
}
