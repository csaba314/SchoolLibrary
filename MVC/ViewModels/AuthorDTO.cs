using Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class AuthorDTO
    {
        [Display(Name = "Author Profile Image")]
        public HttpPostedFileBase UploadedFile { get; set; }

        public int ID { get; set; }

        [Required,
            StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters."),
            Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required,
            StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters."),
            Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Date of Birth"),
            DataType(DataType.Date),
            DisplayFormat(NullDisplayText = "No DOB provided.")]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Date of Death"),
            DataType(DataType.Date),
            DisplayFormat(NullDisplayText = "No DOD provided.")]
        public DateTime? DateOfDeath { get; set; }

        [StringLength(2000),
            Display(Name = "About")]
        public string AboutAuthor { get; set; }

        public int? FileModelId { get; set; }

        public IFileModel FileModel { get; set; }
    }
}