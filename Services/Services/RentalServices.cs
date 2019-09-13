using Common.Parameters;
using PagedList;
using Services.Models;
using Services.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.Services
{
    public class RentalServices : IRentalServices
    {

        private IUnitOfWork _unitOfWork;

        public RentalServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IRental Get(int id)
        {
            var rental = _unitOfWork.Get<Rental>(id);
            return rental;
        }

        public IEnumerable<IRental> GetAllByCustomer(int customerId)
        {
            return _unitOfWork.GetAll<Rental>().Where(r => r.CustomerId == customerId)
                                               .Include(r => r.Book)
                                               .OrderByDescending(r => r.DateRented);
        }

        public IPagedList<IRental> GetAll(ISorting sorting, IFiltering filtering, IPaging paging, IOptions options)
        {
            IQueryable<IRental> rentals = _unitOfWork.GetAll<Rental>();

            if (options.IncludeBooks)
            {
                rentals = rentals.Include(r => r.Book);
            }

            if (options.IncludeCustomers)
            {
                rentals = rentals.Include(r => r.Customer);
            }

            // filter records based on the DateReturned beeing null or not
            switch (filtering.RecordsFilter)
            {
                case "Rented":
                    rentals = rentals.Where(r => r.DateReturned == null);
                    break;
                case "Returned":
                    rentals = rentals.Where(r => r.DateReturned != null);
                    break;
            }

            if (!string.IsNullOrEmpty(filtering.SearchString))
            {
                switch (filtering.SearchBy)
                {
                    case "Book Title":
                        rentals = rentals.Where(r => r.Book.Title.ToLower().Contains(filtering.SearchString.ToLower()));
                        break;
                    case "Customer Name":
                        rentals = rentals.Where(r => r.Customer.LastName.ToLower().Contains(filtering.SearchString.ToLower())
                                                    || r.Customer.FirstName.ToLower().Contains(filtering.SearchString.ToLower()));
                        break;
                    case "Rental Id":
                        rentals = rentals.Where(r => r.Id.ToString().Contains(filtering.SearchString));
                        break;
                }
            }

            switch (sorting.SortingParam)
            {
                case "dRented":
                    rentals = rentals.OrderBy(c => c.DateRented);
                    break;
                case "dReturned":
                    rentals = rentals.OrderBy(c => c.DateRented);
                    break;
                case "dReturned_desc":
                    rentals = rentals.OrderByDescending(c => c.DateRented);
                    break;
                case "bookTitle":
                    rentals = rentals.OrderBy(c => c.Book.Title);
                    break;
                case "bookTitle_desc":
                    rentals = rentals.OrderByDescending(c => c.Book.Title);
                    break;
                case "customerName":
                    rentals = rentals.OrderBy(c => c.Customer.LastName);
                    break;
                case "customerName_desc":
                    rentals = rentals.OrderByDescending(c => c.Customer.LastName);
                    break;
                default:
                    rentals = rentals.OrderByDescending(c => c.DateRented);
                    break;
            }
            var pagedList = rentals.ToPagedList(paging.PageNumber, paging.PageSize);

            if (pagedList.PageCount < pagedList.PageNumber)
            {
                return rentals.ToPagedList(1, paging.PageSize);
            }

            return pagedList;
        }

        public IDictionary<string, string> PopulateCustomersDropDown()
        {

            var listOfCustomers = _unitOfWork.GetAll<Customer>().OrderBy(x => x.LastName);
            var filteredList = new List<Customer>();

            foreach (var item in listOfCustomers)
            {
                if (GetCustomerRentalCapacity(item) > 0)
                {
                    filteredList.Add(item);
                }
            }
            return filteredList
                .Select(x => new { x.Id, x.LastName })
                .AsEnumerable()
                .ToDictionary(x => x.Id.ToString(), y => y.LastName);
        }

        public IDictionary<string, string> PopulateBooksDropDown()
        {
            //var listOfBooks = _unitOfWork.GetAll<Book>().OrderBy(b => b.Title);
            //var filteredList = new List<Book>();
            //foreach (var item in listOfBooks)
            //{
            //    if (GetCustomerRentalCapacity(item) > 0)
            //    {
            //        filteredList.Add(item);
            //    }
            //}


            return _unitOfWork.GetAll<Book>()
                .Where(b => b.NumberInStock - b.RentedBooks > 0)
                .OrderBy(b => b.Title)
                .Select(b => new { b.ID, b.Title })
                .AsEnumerable()
                .ToDictionary(x => x.ID.ToString(), y => y.Title);
        }

        public int GetCustomerRentalCapacity(ICustomer customer)
        {
            int rc = 0;
            switch (customer.CustomerType)
            {
                case CustomerType.Regular:
                    rc = (int)RentalAmount.Regular;
                    break;
                case CustomerType.Premium:
                    rc = (int)RentalAmount.Premium;
                    break;
                default:
                    rc = (int)RentalAmount.Disabled;
                    break;
            }

            return rc - customer.RentedBooks;
        }

        public void ReturnBook(IRental model)
        {
            _unitOfWork.Get<Book>(model.BookId).RentedBooks--;
            _unitOfWork.Get<Customer>(model.CustomerId).RentedBooks--;
            _unitOfWork.Get<Rental>(model.Id).DateReturned = DateTime.Now;
            _unitOfWork.Commit();
        }

        public void Add(IRental rental)
        {
            if (rental is Rental)
            {
                var book = _unitOfWork.Get<Book>(rental.BookId);
                book.RentedBooks++;

                var customer = _unitOfWork.Get<Customer>(rental.CustomerId);
                customer.RentedBooks++;
                _unitOfWork.Add<Rental>(rental as Rental);
                _unitOfWork.Commit();
            }
        }

    }
}
