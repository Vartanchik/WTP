using System.IO;

namespace WTP.BLL.ModelsDto.Azure
{
    public class FileDataDto
    {
        public Stream Stream { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public FileDataDto()
        {
        }

        public FileDataDto(Stream fileStream, string fileType, string fileName = null)
        {
            Stream = fileStream;

            Type = fileType;

            Name = fileName;
        }
    }
}
