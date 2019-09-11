using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Parameters;
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
        private IParameterBuilder _parameterBuilder;

        public BooksController(IBookServices services,
                               IParameterBuilder parameterBuilder,
                               IFileServices fileServices)
        {
            _services = services;
            _parameterBuilder = parameterBuilder;
            _fileServices = fileServices;
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
            _parameterBuilder.Build(model, searchString, sortOrder, pageSize, pageNumber, true, true);

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

            // todo add dropdowns for authors and genres

            return View(model);
        }

        // POST: Book/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

            // todo add dropdowns for authors and genres

            return View(model);
        }

        // GET: Book/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = _services.GetBookWithAuthor((int)id);

            if (book == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<BookDTO>(book);

            if (book.FileModelId != null)
            {
                model.FileModel = _fileServices.Get((int)book.FileModelId);
            }

            // todo add dropdowns for authors and genres

            return View(model);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,SerialNumber,ReleaseDate,AuthorID,GenreID,Publisher,NumberInStock,FileModelId,UploadedFile")]
                                 BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var book = _services.GetBookWithAuthor(model.ID);

                Mapper.Map(model, book);

                if (model.UploadedFile != null)
                {
                    book.FileModelId = _fileServices.CheckDbAndUploadeFile(model.UploadedFile);
                }

                return RedirectToAction("Index");
            }

            // todo add dropdowns for authors and genres

            return View(model);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = _services.GetBookWithAuthor((int)id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var book = _services.GetBookWithAuthor(id);
            _services.Delete(book);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_services.Dispose();
            }
            base.Dispose(disposing);
        }


        //public ActionResult DownloadFile(int id)
        //{
        //    var file = FileModelHelper.GetFile(id);

        //    if (file != null)
        //    {
        //        return File(file.FileContent, file.ContentType);
        //    }

        //    return View();
        //}


        //private void PopulateSelectListForAuthorAndGenre(CreateEditBookViewModel model)
        //{
        //    ViewBag.AuthorDropDown = new SelectList(_services.Authors.GetAuthorsByLastName(), "ID", "FullName", model.AuthorID);
        //    ViewBag.GenreDropDown = new SelectList(unitOfWork.Genres, "ID", "Name", model.GenreID);
        //}
    }
}