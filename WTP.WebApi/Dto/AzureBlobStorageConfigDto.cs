using Microsoft.Extensions.Configuration;

namespace WTP.WebAPI.Dto
{
    public class AzureBlobStorageConfigDto
    {
        public string AccountName { get; set; }

        public string AccountKey { get; set; }

        public string ContainerName { get; set; }

        public string BlobStorageUrl { get; set; }

        public AzureBlobStorageConfigDto(IConfiguration configuration)
        {
            AccountName = configuration["AppSettings:AccountName"];

            AccountKey = configuration["AppSettings:AccountKey"];

            ContainerName = configuration["AppSettings:ContainerName"];

            BlobStorageUrl = configuration["Url:ImageStorageUrl"];
        }

    }
}
