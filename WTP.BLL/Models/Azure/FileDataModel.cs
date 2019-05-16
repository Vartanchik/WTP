using System.IO;

namespace WTP.BLL.Models.Azure
{
    public class FileDataModel
    {
        public Stream Stream { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public FileDataModel()
        {
        }

        public FileDataModel(Stream fileStream, string fileType, string fileName = null)
        {
            Stream = fileStream;

            Type = fileType;

            Name = fileName;
        }
    }
}
