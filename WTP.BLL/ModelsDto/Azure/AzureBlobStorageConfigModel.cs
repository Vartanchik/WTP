using System;
using System.Collections.Generic;
using System.Text;

namespace WTP.BLL.ModelsDto.Azure
{
    public class AzureBlobStorageConfigModel
    {
        public string AccountName { get; set; }

        public string AccountKey { get; set; }

        public string ContainerName { get; set; }

        public string BlobStorageUrl { get; set; }
    }
}
