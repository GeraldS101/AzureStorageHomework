using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorageHomework.Models.ViewModel
{
    public class BlobViewModel
    {
        public IEnumerable<BlobModel> blobList { get; set; }

        public string DefaultTextFileImage { get; set; }
        public string DefaultPDFFileImage { get; set; }
    }
}
