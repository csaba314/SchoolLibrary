using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public interface IBook : IEquatable<IBook>
    {
        #region Properties
        int ID { get; set; }

        string Title { get; set; }

        string Description { get; set; }

        int SerialNumber { get; set; }

        DateTime ReleaseDate { get; set; }

        int AuthorID { get; set; }

        int GenreID { get; set; }

        [DisplayFormat(NullDisplayText = "No Publisher Data")]
        string Publisher { get; set; }

        int NumberInStock { get; set; }

        int RentedBooks { get; set; }

        int? FileModelId { get; set; }
        #endregion

        #region Navigation Properties

        Author Author { get; set; }

        Genre Genre { get; set; }

        FileModel FileModel { get; set; }
        #endregion
    }
}
