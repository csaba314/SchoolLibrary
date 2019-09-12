using Services.Models;
using Services.UnitOfWork;
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

        public IEnumerable<IRental> GetAll()
        {
            return new List<IRental>();
        }

        public IEnumerable<IRental> GetAllByCustomer(int customerId)
        {
            return _unitOfWork.GetAll<Rental>().Where(r => r.CustomerId == customerId)
                                               .Include(r => r.Book)
                                               .OrderByDescending(r => r.DateRented);
        }

    }
}
