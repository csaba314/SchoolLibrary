using Common.Parameters;
using PagedList;
using Services.Models;
using Services.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class AuthorServices : IAuthorServices
    {
        private IUnitOfWork _unitOfWork;

        public AuthorServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IPagedList<IAuthor> GetAll(ISorting sorting, IFiltering filtering, IPaging paging, IOptions options)
        {
            IQueryable<IAuthor> authors = _unitOfWork.GetAll<Author>();
            
            if (!string.IsNullOrEmpty(filtering.SearchString))
            {
                authors = authors.Where(x => x.FirstName.ToLower().Contains(filtering.SearchString.ToLower())
                                          || x.LastName.ToLower().Contains(filtering.SearchString.ToLower()));
            }

            switch (sorting.SortingParam)
            {
                case "lName_desc":
                    authors = authors.OrderByDescending(a => a.LastName);
                    break;
                case "fName":
                    authors = authors.OrderBy(a => a.FirstName);
                    break;
                case "fName_desc":
                    authors = authors.OrderByDescending(a => a.FirstName);
                    break;
                default:
                    authors = authors.OrderBy(a => a.LastName);
                    break;
            }

            var pagedList = authors.ToPagedList(paging.PageNumber, paging.PageSize);

            if (pagedList.PageCount < pagedList.PageNumber)
            {
                return authors.ToPagedList(1, paging.PageSize);
            }

            return pagedList;


        }

        public IEnumerable<IAuthor> GetAuthorsByLastName()
        {
            return _unitOfWork.GetAll<Author>().OrderBy(x => x.LastName).ToList();
        }

        public IAuthor GetAuthorWithBooks(int id)
        {
            var author = _unitOfWork.Get<Author>(id);
            author.Books = _unitOfWork.GetAll<Book>().Where(x => x.AuthorID == author.ID).ToList();

            return author;
        }

        public void Add(IAuthor author)
        {
            if (author is Author)
            {
                _unitOfWork.Add<Author>(author as Author);
                _unitOfWork.Commit();
            }
        }

        public void Update(IAuthor author)
        {
            if (author is Author)
            {
                _unitOfWork.Update<Author>(author as Author);
                _unitOfWork.Commit();
            }
        }

        public void Delete(IAuthor author)
        {
            if (author is Author)
            {
                author.Books = _unitOfWork.GetAll<Book>().Where(x => x.AuthorID == author.ID).ToList();
                foreach (var item in author.Books)
                {
                    item.AuthorID = 0;
                    _unitOfWork.Update<Book>(item as Book);
                }
                _unitOfWork.Delete<Author>(author as Author);
                _unitOfWork.Commit();
            }
        }
    }
}
