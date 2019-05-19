namespace WTP.BLL.DTOs.AzureDTOs
{
    public class AzureBlobStorageConfigDto
    {
        public AzureBlobStorageConfigDto(string accName, string accKey, string containerName, string storageUrl)
        {
            AccountName = accName;
            AccountKey = accKey;
            ContainerName = containerName;
            BlobStorageUrl = storageUrl;
        }

        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ContainerName { get; set; }
        public string BlobStorageUrl { get; set; }
    }
}
