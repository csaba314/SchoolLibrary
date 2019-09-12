using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MVC.ViewModels;
using Services.Models;
using Services.Services;
using AutoMapper;
using System.Net;

namespace MVC.Controllers
{
    public class BooksController : Controller
    {

        private IBookServices _services;
        private IFileServices _fileServices;
        private IAuthorServices _authorServices;
        private IGenreServices _genreServices;
        private IParameterBuilder _parameterBuilder;

        public BooksController(IBookServices services,
                               IAuthorServices authorServices,
                               IGenreServices genreServices,
                               IParameterBuilder parameterBuilder,
                               IFileServices fileServices)
        {
            _services = services;
            _authorServices = authorServices;
            _genreServices = genreServices;
            _fileServices = fileServices;
            _parameterBuilder = parameterBuilder;
            
        }

        // GET: Book
        [HttpGet]
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int id = 0, int pageNumber = 1, int pageSize = 10)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var model = DependencyResolver.Current.GetService<BookIndexViewModel>();
            _parameterBuilder.Build(model, searchString, sortOrder, pageSize, pageNumber, includeAuthors: true, includeGenres: true);

            model.Books = _services.GetBooksWithAuthorsAndGenres(model.Sorting, model.Filtering, model.Paging, model.Options);

            model.Id = id > 0 ? id : 0;

            ViewBag.TitleSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AuthorSortParam = sortOrder == "Author" ? "author_desc" : "Author";
            ViewBag.GenreSortParam = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.PageSizeDropdown = new SelectList(new List<int>() { 5, 10, 20, 40 }, model.Paging.PageNumber);

            return View(model);
        }


        // GET: Book/Create
        public ActionResult Create()
        {
            var model = DependencyResolver.Current.GetService<BookDTO>();

            ViewBag.AuthorsDropdown = new SelectList(_authorServices.GetAuthorsByLastName(), "ID", "FullName");
            ViewBag.GenresDropdown = new SelectList(_genreServices.GetAll(), "ID", "Name");

            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,SerialNumber,ReleaseDate,AuthorID,GenreID,Publisher,NumberInStock,UploadedFile")]
                                    BookDTO model)
        {
            if (ModelState.IsValid)
            {

                var book = DependencyResolver.Current.GetService<IBook>();
                Mapper.Map(model, book);

                if (model.UploadedFile != null)
                {
                    book.FileModelId = _fileServices.CheckDbAndUploadeFile(model.UploadedFile);
                }

                _services.Add(book);
                return RedirectToAction("Index");
            }

            ViewBag.AuthorsDropdown = new SelectList(_authorServices.GetAuthorsByLastName(), "ID", "FullName");
            ViewBag.GenresDropdown = new SelectList(_genreServices.GetAll(), "ID", "Name");

            return View(model);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = _services.Get((int)id);

            if (book == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<BookDTO>(book);

            if (book.FileModelId != null)
            {
                model.FileModel = _fileServices.Get((int)book.FileModelId);
            }

            ViewBag.AuthorsDropdown = new SelectList(_authorServices.GetAuthorsByLastName(), "ID", "FullName", model.AuthorID);
            ViewBag.GenresDropdown = new SelectList(_genreServices.GetAll(), "ID", "Name", model.GenreID);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,SerialNumber,ReleaseDate,AuthorID,GenreID,Publisher,NumberInStock,FileModelId,UploadedFile")]
                                 BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var book = _services.Get(model.ID);

                Mapper.Map(model, book);

                if (model.UploadedFile != null)
                {
                    book.FileModelId = _fileServices.CheckDbAndUploadeFile(model.UploadedFile);
                }

                _services.Update(book);

                return RedirectToAction("Index");
            }

            ViewBag.AuthorsDropdown = new SelectList(_authorServices.GetAuthorsByLastName(), "ID", "FullName", model.AuthorID);
            ViewBag.GenresDropdown = new SelectList(_genreServices.GetAll(), "ID", "Name", model.GenreID);

            return View(model);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = _services.Get((int)id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<BookDTO>(book));
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = _services.Get(id);
            _services.Delete(book);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}