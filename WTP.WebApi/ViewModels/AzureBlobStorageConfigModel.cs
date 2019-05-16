using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.WebAPI.ViewModels
{
    public class AzureBlobStorageConfigModel
    {
        public string AccountName { get; set; }

        public string AccountKey { get; set; }

        public string ContainerName { get; set; }

        public string BlobStorageUrl { get; set; }

        public AzureBlobStorageConfigModel(IConfiguration configuration)
        {
            AccountName = configuration["AppSettings:AccountName"];

            AccountKey = configuration["AppSettings:AccountKey"];

            ContainerName = configuration["AppSettings:ContainerName"];

            BlobStorageUrl = configuration["Url:ImageStorageUrl"];
        }

    }
}
