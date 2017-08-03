using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace AzureStorageHomework.Models
{
    public class ConstantModel
    {
        IConfiguration _configuration;
        IHostingEnvironment _env;
        public ConstantModel(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            CloudStorageConnectionString = _configuration.GetSection("AppSettings")["ConnectionString"];
            ContainerName = _configuration.GetSection("AppSettings")["Container"];
            EmployeeTable = _configuration.GetSection("AppSettings")["EmployeeTable"];
            RoleTable = _configuration.GetSection("AppSettings")["RoleTable"];
            UploadFolder = _env.WebRootPath + @"\images\Upload\";
        }
        public string CloudStorageConnectionString = string.Empty;
        public string ContainerName = string.Empty;
        public string EmployeeTable = string.Empty;
        public string RoleTable = string.Empty;
        public string UploadFolder = string.Empty;
        public string ServerName = "http://localhost:49731/";
        public string DefaultTextFileImage = @"\images\Upload\text-document.png";
        public string DefaultPdfImage = @"\images\Upload\pdf-image.jpg";
    }
}
