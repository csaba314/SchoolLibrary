
using Common.Parameters;
using PagedList;
using Services.Models;
using System.Web.Mvc;

namespace MVC.ViewModels
{
    public class RentalsIndexViewModel : IParameterizedViewModel
    {
        public IPagedList<IRental> Rentals { get; set; }

        public RentalDTO Rental { get; set; }

        public IFiltering Filtering { get; set; }
        public ISorting Sorting { get; set; }
        public IPaging Paging { get; set; }
        public IOptions Options { get; set; }

        public SelectList BooksDropDown { get; set; }
    }
}