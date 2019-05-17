using System.IO;

namespace WTP.WebAPI.Dto
{
    public class FileDataDto
    {
        public Stream Stream { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string BlobName { get; set; }

        public FileDataDto()
        {
        }

        public FileDataDto(Stream fileStream, string fileType, string fileName, string blobName = null)
        {
            Stream = fileStream;

            Type = fileType;

            BlobName = blobName;

            Name = fileName;
        }
    }
}
