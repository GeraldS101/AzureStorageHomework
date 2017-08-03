using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageHomework.Models
{
    public class BlobModel
    {
        // File extension array
        string[] imageFile = new string[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        string[] textFile = new string[] { ".txt", ".csv", ".xls", ".xlsx" };
        //public IEnumerable<IListBlobItem> BlobList { get; set; }
        public string fileURL { get; set; }
        public string fileName
        {
            get
            {
                return Path.GetFileName(fileURL);
            }
        }
        public string Container
        {
            get
            {
                return Path.GetFileName(Path.GetDirectoryName(fileURL));
            }
        }
        public string fileExtension {
            get
            {
                return Path.GetExtension(fileURL);
            }
        }

        public bool isTextFile
        {
            get
            {
                return textFile.Contains(fileExtension.ToLower());
            }
        }

        public bool isImageFile
        {
            get
            {
                return imageFile.Contains(fileExtension.ToLower());
            }
        }

        public bool isPDFFile
        {
            get
            {
                return fileExtension.ToLower() == ".pdf";
            }
        }
    }
}
