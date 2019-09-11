using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    interface IFileModel
    {
        int FileModelId { get; set; }

        [Display(Name = "File Name")]
        string FileName { get; set; }

        string FileType { get; set; }

        string FileContent { get; set; }

        DateTime DateAdded { get; set; }
    }
}
