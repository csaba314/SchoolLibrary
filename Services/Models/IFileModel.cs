using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public interface IFileModel
    {
        int FileModelId { get; set; }

        [Display(Name = "File Name")]
        string FileName { get; set; }

        string FileType { get; set; }

        string FileContent { get; set; }

        DateTime DateAdded { get; set; }
    }
}
