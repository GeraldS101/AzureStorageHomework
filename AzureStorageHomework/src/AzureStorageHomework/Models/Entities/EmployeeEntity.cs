using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageHomework.Models.Entities
{
    public class EmployeeEntity : TableEntity
    {
        //public EmployeeEntity(string lastName, string firstName)
        //{
        //    this.PartitionKey = lastName;
        //    this.RowKey = firstName;


        //    this.LastName = lastName;
        //    this.FirstName = firstName;
        //}

        public EmployeeEntity(Guid id)
        {
            this.PartitionKey = id.ToString();
            this.RowKey = id.ToString();


            this.Id = id;
        }

        public EmployeeEntity() { }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateHired { get; set; }
        public bool isActive { get; set; }
        public int RoleId { get; set; }
    }
}
