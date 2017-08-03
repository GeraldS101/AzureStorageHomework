using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageHomework.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateHired { get; set; }
        public bool isActive { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
