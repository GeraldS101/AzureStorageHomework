using AzureStorageHomework.Models;
using AzureStorageHomework.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageHomework.Services
{
    public class BlobService : IBlobService
    {
        IConfiguration _configuration;
        IHostingEnvironment _env;

        ConstantModel constantModel;
        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer container;

        public BlobService(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            constantModel = new ConstantModel(_configuration, _env);

            // Retrieve storage account from connection string.
            storageAccount = CloudStorageAccount.Parse(constantModel.CloudStorageConnectionString);

            // Create the blob client.
            blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            container = blobClient.GetContainerReference(constantModel.ContainerName);
        }
        public async Task<BlobViewModel> GetBlobList()
        {
            // Create the container if it doesn't already exist.
            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
            }

            // get list of blobs in azure
            var listBlobItems = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 200, null, null, null);
            var blobList = listBlobItems.Results.Select(a => new BlobModel
            {
                fileURL = a.Uri.AbsoluteUri
            });

            BlobViewModel model = new BlobViewModel();
            model.blobList = blobList;
            model.DefaultTextFileImage = constantModel.ServerName + constantModel.DefaultTextFileImage;
            model.DefaultPDFFileImage = constantModel.ServerName + constantModel.DefaultPdfImage;

            return model;
        }
        public async Task<bool> DeleteBlob(string name)
        {
            // Get a reference to a blob  
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

            // Delete the blob if it is existing 
            return await blockBlob.DeleteIfExistsAsync();
        }
        public async Task<bool> CreateBlob(IFormFile file1)
        {
            return await UploadBlob(file1);
        }

        public async Task<bool> UpdateBlob(IFormFile file1, string name)
        {
            return await UploadBlob(file1, name);
        }
        private async Task<bool> UploadBlob(IFormFile file1, string name = "")
        {
            string filename = constantModel.UploadFolder + file1.FileName;
            try
            {
                using (FileStream fs = File.Create(filename))
                {
                    await file1.CopyToAsync(fs);
                    fs.Flush();
                }

                if (filename.Length > 0)
                {
                    // write a blob to the container
                    var blobName = name.Length > 0 ? name : file1.FileName;
                    CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
                    blob.Metadata.Add("FileName", file1.FileName);
                    blob.Metadata.Add("FileExtension", Path.GetExtension(filename));
                    await blob.UploadFromFileAsync(filename);

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }
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
