using Common.Parameters;
using PagedList;
using Services.Models;
using System.Collections.Generic;

namespace Services.Services
{
    public interface IBookServices
    {
        IPagedList<IBook> GetBooksWithAuthorsAndGenres(ISorting sorting, IFiltering filtering, IPaging paging, IOptions options);
        IBook Get(int id);
        IEnumerable<IBook> GetBooksByAuthor(int authorId);
        void Add(IBook book);
        void Update(IBook book);
        void Delete(IBook book);

    }
}
