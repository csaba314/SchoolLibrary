using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class FileHelper : IFileHelper
    {
        public FileHelper(byte[] fileContent, string fileType) : this(fileContent, fileType, null) { }

        public FileHelper(byte[] fileContent, string fileType, string fileName)
        {
            this.FileContent = fileContent;
            this.ContentType = fileType;
            this.FileName = fileName;
        }

        public byte[] FileContent { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}
