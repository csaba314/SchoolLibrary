using Common.Parameters;
using PagedList;
using Services.Models;
using Services.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


namespace Services.Services
{
    public class CustomerServices : ICustomerServices
    {

        private IUnitOfWork _unitOfWork;

        public CustomerServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ICustomer Get(int id)
        {
            return _unitOfWork.Get<Customer>(id);
        }

        public IPagedList<ICustomer> GetAll(out IEnumerable<IRental> rentals, ISorting sorting, IFiltering filtering, IPaging paging, IOptions options)

        {
            IQueryable<ICustomer> customers = _unitOfWork.GetAll<Customer>();

            if (!String.IsNullOrEmpty(filtering.SearchString))
            {
                customers = customers.Where(c => c.FirstName.ToLower().Contains(filtering.SearchString.ToLower())
                                              || c.LastName.ToLower().Contains(filtering.SearchString.ToLower()));
            }

            rentals = _unitOfWork.GetAll<Rental>()
                .Include(r => r.Book)
                .Where(r => r.CustomerId == options.Id)
                .OrderByDescending(x => x.DateRented);


            if ((options.IncludeRentalHistory == false) && (rentals != null))
            {
                rentals = rentals.Where(r => r.DateReturned == null);
            }

            switch (sorting.SortingParam)
            {
                case "lName_desc":
                    customers = customers.OrderByDescending(c => c.LastName);
                    break;
                case "fName":
                    customers = customers.OrderBy(c => c.FirstName);
                    break;
                case "fName_desc":
                    customers = customers.OrderByDescending(c => c.FirstName);
                    break;
                case "accountNo":
                    customers = customers.OrderBy(c => c.AccountNumber);
                    break;
                case "accountNo_desc":
                    customers = customers.OrderByDescending(c => c.AccountNumber);
                    break;
                default:
                    customers = customers.OrderBy(c => c.LastName);
                    break;
            }

            var pagedList = customers.ToPagedList(paging.PageNumber, paging.PageSize);

            if (pagedList.PageCount < pagedList.PageNumber)
            {
                return customers.ToPagedList(1, paging.PageSize);
            }
            return pagedList;

        }

        public void Add(ICustomer customer)
        {
            if (customer is Customer)
            {
                _unitOfWork.Add<Customer>(customer as Customer);
                _unitOfWork.Commit();
            }
        }

        public void Update(ICustomer customer)
        {
            if (customer is Customer)
            {
                _unitOfWork.Update<Customer>(customer as Customer);
                _unitOfWork.Commit();
            }
        }

        public IEnumerable<int> GetCustomerAccountNumbers()
        {
            return _unitOfWork.GetAll<Customer>().Select(c => c.AccountNumber).ToList();
        }

        public int GenerateAccountNumber()
        {
            int accNo = 0;
            bool flag = true;
            var rng = new Random();
            var existingAccountNumbers = GetCustomerAccountNumbers();

            while (flag)
            {
                accNo = rng.Next(10000000, 99999999);

                if (!existingAccountNumbers.Contains(accNo))
                {
                    flag = false;
                }
            }
            return accNo;
        }
    }
}
