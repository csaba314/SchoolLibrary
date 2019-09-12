using System.Collections.Generic;
using Services.Models;

namespace Services.Services
{
    public interface IGenreServices
    {
        void Add(IGenre genre);
        void Delete(IGenre genre);
        IGenre Get(int id);
        IEnumerable<IGenre> GetAll();
        void Update(IGenre genre);
    }
}