using Services.Models;
using Services.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class GenreServices : IGenreServices
    {
        private IUnitOfWork _unitOfWork;

        public GenreServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<IGenre> GetAll()
        {
            return _unitOfWork.GetAll<Genre>().ToList();
        }

        public IGenre Get(int id)
        {
            return _unitOfWork.Get<Genre>(id);
        }

        public void Add(IGenre genre)
        {
            if (genre is Genre)
            {
                _unitOfWork.Add<Genre>(genre as Genre);
            }
        }

        public void Update(IGenre genre)
        {
            if (genre is Genre)
            {
                _unitOfWork.Update<Genre>(genre as Genre);
            }
        }

        public void Delete(IGenre genre)
        {
            if (genre is Genre)
            {
                _unitOfWork.Delete<Genre>(genre as Genre);
            }
        }
    }
}
