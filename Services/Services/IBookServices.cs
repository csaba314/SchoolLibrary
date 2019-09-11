using Common.Parameters;
using PagedList;
using Services.Models;

namespace Services.Services
{
    public interface IBookServices
    {
        IPagedList<IBook> GetBooksWithAuthorsAndGenres(ISorting sorting, IFiltering filtering, IPaging paging, IOptions options);
        IBook GetBookWithAuthor(int id);
        void Add(IBook book);
        void Update(IBook book);
        void Delete(IBook book);

    }
}
