using System.Collections.Generic;
using Common.Parameters;
using PagedList;
using Services.Models;

namespace Services.Services
{
    public interface IAuthorServices
    {
        IPagedList<IAuthor> GetAll(ISorting sorting, IFiltering filtering, IPaging paging, IOptions options);
        IEnumerable<IAuthor> GetAuthorsByLastName();
        IAuthor GetAuthorWithBooks(int id);
        void Add(IAuthor author);
        void Update(IAuthor author);
        void Delete(IAuthor author);
    }
}