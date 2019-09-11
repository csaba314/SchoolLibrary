using System.Data.Entity;
using System.Linq;
using Common.Parameters;
using PagedList;
using Services.Models;
using Services.UnitOfWork;

namespace Services.Services
{
    public class BookServices : IBookServices
    {
        IUnitOfWork _unitOfWork;
        
        public BookServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IPagedList<IBook> GetBooksWithAuthorsAndGenres(ISorting sorting, IFiltering filtering, IPaging paging, IOptions options)
        {
            IQueryable<Book> books = _unitOfWork.GetAll<Book>();

            if (options.IncludeAuthors)
            {
                books = books.Include(x => x.Author);
            }

            if (options.IncludeGenres)
            {
                books = books.Include(x => x.Genre);
            }

            if (!string.IsNullOrEmpty(filtering.SearchString))
            {
                books = books.Where(x => x.Title.ToLower().Contains(filtering.SearchString.ToLower()));
            }

            switch (sorting.SortingParam)
            {
                case "title_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                case "Author":
                    books = books.OrderBy(b => b.Author.LastName);
                    break;
                case "author_desc":
                    books = books.OrderByDescending(b => b.Author.LastName);
                    break;
                case "Genre":
                    books = books.OrderBy(b => b.Genre.Name);
                    break;
                case "genre_desc":
                    books = books.OrderByDescending(b => b.Genre.Name);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            var pagedList = books.ToPagedList(paging.PageNumber, paging.PageSize);

            if (pagedList.PageCount < pagedList.PageNumber)
            {
                return books.ToPagedList(1, paging.PageSize);
            }

            return pagedList;
        }

        public IBook GetBookWithAuthor(int id)
        {
            return _unitOfWork.Get<Book>(id);
        }

        public void Add(IBook book)
        {
            if (book is Book)
            {
                _unitOfWork.Add<Book>(book as Book);
                _unitOfWork.Commit();
            }
        }

        public void Update(IBook book)
        {
            if (book is Book)
            {
                _unitOfWork.Update<Book>(book as Book);
                _unitOfWork.Commit();
            }
        }

        public void Delete(IBook book)
        {
            if (book is Book)
            {
                _unitOfWork.Delete<Book>(book as Book);
                _unitOfWork.Commit();
            }
        }
        
    }
}
