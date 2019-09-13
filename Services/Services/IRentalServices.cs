using System.Collections.Generic;
using Common.Parameters;
using PagedList;
using Services.Models;

namespace Services.Services
{
    public interface IRentalServices
    {
        IRental Get(int id);
        IPagedList<IRental> GetAll(ISorting sorting, IFiltering filtering, IPaging paging, IOptions options);
        IEnumerable<IRental> GetAllByCustomer(int customerId);
        void Add(IRental rental);
        void ReturnBook(IRental model);
        int GetCustomerRentalCapacity(ICustomer customer);
        IDictionary<string, string> PopulateBooksDropDown();
        IDictionary<string, string> PopulateCustomersDropDown();
        
    }
}