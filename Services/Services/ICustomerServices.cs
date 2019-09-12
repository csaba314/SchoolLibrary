using Common.Parameters;
using PagedList;
using Services.Models;
using System.Collections.Generic;

namespace Services.Services
{
    public interface ICustomerServices
    {
        ICustomer Get(int id);
        IPagedList<ICustomer> GetAll(out IEnumerable<IRental> rentals, ISorting sorting, IFiltering filtering, IPaging paging, IOptions options);
        void Add(ICustomer customer);
        void Update(ICustomer customer);
        IEnumerable<int> GetCustomerAccountNumbers();
        int GenerateAccountNumber();
    }
}