using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Parameters;
using PagedList;
using Services.Models;

namespace MVC.ViewModels
{
    public class RentalsIndexViewModel : IParameterizedViewModel
    {
        public IPagedList<IRental> Rentals { get; set; }

        public SelectList SearchParamDropdown { get; set; }
        public SelectList PagingParamDropdown { get; set; }
        public SelectList RecordsFilterDropdown { get; set; }

        public RentalDTO Rental { get; set; }

        public IFiltering Filtering { get; set; }
        public ISorting Sorting { get; set; }
        public IPaging Paging { get; set; }
        public IOptions Options { get; set; }

    }
}