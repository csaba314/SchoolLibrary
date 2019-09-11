using System.Collections.Generic;

namespace Services.Models
{
    public enum CustomerType
    {

        Disabled = 0,
        Regular = 1,
        Premium = 2
    }

    public interface ICustomer
    {
        int Id { get; set; }

        int AccountNumber { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Address { get; set; }

        string PhoneNumber { get; set; }

        int RentedBooks { get; set; }

        string FullName { get; }

        CustomerType CustomerType { get; set; }

        ICollection<IRental> Rentals { get; set; }

    }
}
