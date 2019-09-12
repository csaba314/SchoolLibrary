using Common.Parameters;
using PagedList;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.ViewModels
{
    public class CustomerIndexViewModel : IParameterizedViewModel
    {
        public IPagedList<ICustomer> Customers { get; set; }
        public IEnumerable<IRental> Rentals { get; set; }

        public IFiltering Filtering { get; set; }
        public ISorting Sorting { get; set; }
        public IPaging Paging { get; set; }
        public IOptions Options { get; set; }
    }
}