using Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required,
            StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters."),
            Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required,
            StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters."),
            Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required,
            StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        [Required,
            StringLength(maximumLength: 10, MinimumLength = 9, ErrorMessage = "Phone number must be between 9 and 10 digits."),
            Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Rented Books")]
        public int RentedBooks { get; set; }

        [Required,
            Display(Name = "Customer Type")]
        public CustomerType CustomerType { get; set; }
    }
}