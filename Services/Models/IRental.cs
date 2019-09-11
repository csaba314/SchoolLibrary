using System;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public enum RentalAmount
    {
        Disabled = 0,
        Regular = 4,
        Premium = 6
    }

    public interface IRental
    {
        int Id { get; set; }

        int BookId { get; set; }

        int CustomerId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        DateTime DateRented { get; set; }

        [DisplayFormat(NullDisplayText = "Book is still rented.", DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        DateTime? DateReturned { get; set; }

        Customer Customer { get; set; }

        Book Book { get; set; }
    }
}
