using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class FileModel : IFileModel
    {
        public int FileModelId { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileContent { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
