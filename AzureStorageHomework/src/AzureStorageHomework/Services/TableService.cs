using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorageHomework.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using AzureStorageHomework.Models.ViewModel;
using Microsoft.WindowsAzure.Storage.Table;
using AzureStorageHomework.Models.Entities;

namespace AzureStorageHomework.Services
{
    public class TableService : ITableService
    {
        IConfiguration _configuration;
        IHostingEnvironment _env;

        ConstantModel constantModel;
        CloudStorageAccount storageAccount;
        CloudTableClient tableClient;
        CloudTable employeeTable;

        public TableService(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            constantModel = new ConstantModel(_configuration, _env);

            // Retrieve storage account from connection string.
            storageAccount = CloudStorageAccount.Parse(constantModel.CloudStorageConnectionString);

            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            employeeTable = tableClient.GetTableReference(constantModel.EmployeeTable);
        }
        public async Task<EmployeeViewModel> GetEmployeeList()
        {
            // Create the table if it doesn't exist.
            await employeeTable.CreateIfNotExistsAsync();

            // Initialize a default TableQuery to retrieve all the entities in the table.
            TableQuery<EmployeeEntity> tableQuery = new TableQuery<EmployeeEntity>();

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;
            TableQuerySegment<EmployeeEntity> tableQueryResult;
            List<EmployeeEntity> result;

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                tableQueryResult =
                    await employeeTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;

                // Print the number of rows retrieved.
                //Console.WriteLine("Rows retrieved {0}", tableQueryResult.Results.Count);

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);


            result = tableQueryResult.Results;

            var employeeList = result.Select(a => new EmployeeModel
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                DateHired = a.DateHired,
                isActive = a.isActive,
                RoleId = a.RoleId

            });

            var model = new EmployeeViewModel();
            model.Employees = employeeList;

            return model;
        }


        public async Task<EmployeeModel> GetEmployeeDetails(string id)
        {
            // Create a retrieve operation that takes a employee entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<EmployeeEntity>(id, id);

            // Execute the retrieve operation.
            TableResult retrievedResult = await employeeTable.ExecuteAsync(retrieveOperation);

            var result = ((EmployeeEntity)retrievedResult.Result);
            var employee = new EmployeeModel
                            {
                                Id = result.Id,
                                EmployeeId = result.EmployeeId,
                                FirstName = result.FirstName,
                                LastName = result.LastName,
                                DateHired = result.DateHired.Date,
                                isActive = result.isActive,
                                RoleId = result.RoleId

                            };

            return employee;
        }
        public async Task<bool> CreateEntry(EmployeeModel employee)
        {
            try
            {
                // Create a employee entity.
                var id = Guid.NewGuid();
                EmployeeEntity emp = new EmployeeEntity(id);
                emp.EmployeeId = employee.EmployeeId;
                emp.FirstName = employee.FirstName;
                emp.LastName = employee.LastName;
                emp.DateHired = employee.DateHired;
                emp.isActive = employee.isActive;
                emp.RoleId = employee.RoleId;

                // Create the TableOperation object that inserts the employee entity.
                TableOperation insertOperation = TableOperation.Insert(emp);

                // Execute the operation.
                await employeeTable.ExecuteAsync(insertOperation);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteEntry(string id)
        {
            try
            {
                // Create a retrieve operation that expects a employee entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<EmployeeEntity>(id, id);

                // Execute the operation.
                TableResult retrievedResult = await employeeTable.ExecuteAsync(retrieveOperation);

                // Assign the result to a EmployeeEntity.
                EmployeeEntity deleteEntity = (EmployeeEntity)retrievedResult.Result;

                // Create the Delete TableOperation.
                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                    // Execute the operation.
                    await employeeTable.ExecuteAsync(deleteOperation);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateEntry(EmployeeModel employee)
        {
            try
            {
                // Create a retrieve operation that takes a employee entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<EmployeeEntity>(employee.Id.ToString(), employee.Id.ToString());

                // Execute the operation.
                var retrievedResult = await employeeTable.ExecuteAsync(retrieveOperation);

                // Assign the result to a EmployeeEntity object.
                EmployeeEntity updateEntity = (EmployeeEntity)retrievedResult.Result;

                if (updateEntity != null)
                {
                    // Change column details
                    updateEntity.EmployeeId = employee.EmployeeId;
                    updateEntity.FirstName = employee.FirstName;
                    updateEntity.LastName = employee.LastName;
                    updateEntity.DateHired = employee.DateHired;
                    updateEntity.isActive = employee.isActive;
                    updateEntity.RoleId = employee.RoleId;

                    // Create the Replace TableOperation.
                    TableOperation updateOperation = TableOperation.Replace(updateEntity);

                    // Execute the operation.
                    await employeeTable.ExecuteAsync(updateOperation);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
