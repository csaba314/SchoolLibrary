using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public interface IAuthor
    {
        int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters."), Display(Name = "Last Name")]
        string LastName { get; set; }

        [Required, StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters."), Display(Name = "First Name")]
        string FirstName { get; set; }

        [Display(Name = "Date of Birth"),
            DataType(DataType.Date),
            DisplayFormat(NullDisplayText = "No DOB provided.")]
        DateTime? DateOfBirth { get; set; }

        [Display(Name = "Date of Death"),
            DataType(DataType.Date),
            DisplayFormat(NullDisplayText = "No DOD provided.")]
        DateTime? DateOfDeath { get; set; }

        [StringLength(2000),
            Display(Name = "About"),
            DisplayFormat(NullDisplayText = "No data available")]
        string AboutAuthor { get; set; }

        int? FileModelId { get; set; }

        [Display(Name = "Full Name")]
        string FullName { get; }

        FileModel FileModel { get; set; }

        IEnumerable<IBook> Books { get; set; }
    }
}
