using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class Rental : IRental
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int CustomerId { get; set; }

        public DateTime DateRented { get; set; }

        public DateTime? DateReturned { get; set; }



        public Customer Customer { get; set; }

        public Book Book { get; set; }
    }
}
