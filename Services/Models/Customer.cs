using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }

        [Range(10000000, 99999999)]
        public int AccountNumber { get; set; }

        [Required,
            StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required,
            StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required,
            StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        [Required,
            StringLength(maximumLength: 10, MinimumLength = 9, ErrorMessage = "Phone number must be between 9 and 10 digits.")]
        public string PhoneNumber { get; set; }

        public int RentedBooks { get; set; }

        public CustomerType CustomerType { get; set; }

        public ICollection<IRental> Rentals { get; set; }

        public string FullName { get { return String.Format("{0}, {1}", LastName, FirstName); } }
    }
}
