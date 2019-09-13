using Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.ViewModels
{
    public class RentalDTO
    {
        public int Id { get; set; }

        [Display(Name = "Select A Book")]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Select A Customer")]
        public int CustomerId { get; set; }

        public DateTime DateRented { get; set; }

        public DateTime? DateReturned { get; set; }

        public int CustomerRentalCapacity { get; set; }


        [DisplayFormat(NullDisplayText = "No customer selected")]
        public ICustomer Customer { get; set; }

        public IBook Book { get; set; }

        public IEnumerable<SelectListItem> BooksDropdown { get; set; }

        public IEnumerable<SelectListItem> Customersdropdown { get; set; }


        public ICollection<IBook> BooksToRent { get; set; }
    }
}