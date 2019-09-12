using Services.Models;
using System.Collections.Generic;

namespace Services.Services
{
    public interface IRentalServices
    {
        IEnumerable<IRental> GetAll();
        IEnumerable<IRental> GetAllByCustomer(int customerId);
        
    }
}