using Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class BookDTO
    {
        [Display(Name = "Book Cover Image")]
        public HttpPostedFileBase UploadedFile { get; set; }

        public int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Title cannot be longer than 50 characters.")]
        public string Title { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }

        [Required, Range(100000000, 999999999, ErrorMessage = "Serial number must be 9 digits long.")]
        [Display(Name = "Serial Number")]
        public int SerialNumber { get; set; }

        [Required, Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required, Display(Name = "Author")]
        public int AuthorID { get; set; }

        [Required, Display(Name = "Genre")]
        public int GenreID { get; set; }

        [DisplayFormat(NullDisplayText = "No data")]
        public string Publisher { get; set; }

        [Required, Display(Name = "Number in Stock"), Range(0, 20)]
        public int NumberInStock { get; set; }

        [Required,
            Display(Name = "Rented Books")]
        public int RentedBooks { get; set; }

        public int? FileModelId { get; set; }

        public IFileModel FileModel { get; set; }

        public IAuthor Author { get; set; }

        public IGenre Genre { get; set; }
    }
}