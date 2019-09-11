using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Parameters;
using MVC.ViewModels;
using Services.Services;

namespace MVC.Controllers
{
    public class BooksController : Controller
    {

        private IBookServices _services;
        private IParameterBuilder _parameterBuilder;

        public BooksController(IBookServices services,
                               IParameterBuilder parameterBuilder)
        {
            _services = services;
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
            _parameterBuilder.Build(model, searchString, sortOrder, pageSize, pageNumber, true, true);

            model.Books = _services.GetBooksWithAuthorsAndGenres(model.Sorting, model.Filtering, model.Paging, model.Options);

            model.Id = id > 0 ? id : 0;

            ViewBag.TitleSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AuthorSortParam = sortOrder == "Author" ? "author_desc" : "Author";
            ViewBag.GenreSortParam = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.PageSizeDropdown = new SelectList(new List<int>() { 5, 10, 20, 40 }, model.Paging.PageNumber);

            return View(model);
        }


        //// GET: Book/Create
        //public ActionResult Create()
        //{
        //    var model = this.modelBuilder.BuildCreateEditBookViewModel();
        //    return View(model);
        //}

        //// POST: Book/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Title,Description,SerialNumber,ReleaseDate,AuthorID,GenreID,Publisher,NumberInStock,UploadedFile")]
        //                            CreateEditBookViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var book = new Book();
        //        Mapper.Map(model, book);

        //        if (model.UploadedFile != null)
        //        {
        //            book.FileModelId = FileModelHelper.CheckDbAndUploadeFile(model.UploadedFile);
        //        }

        //        unitOfWork.Books.Add(book);
        //        unitOfWork.Save();

        //        return RedirectToAction("Index");
        //    }


        //    return View(modelBuilder.RebuildCreateEditBookViewModel(model));
        //}

        //// GET: Book/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var book = unitOfWork.Books.Find(b => b.ID == id).SingleOrDefault();

        //    if (book == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var model = modelBuilder.BuildCreateEditBookViewModel();
        //    Mapper.Map(book, model);

        //    if (book.FileModelId != null)
        //    {
        //        model.FileModel = unitOfWork.Files.First(f => f.FileModelId == book.FileModelId);
        //    }

        //    return View(model);
        //}

        //// POST: Book/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Title,Description,SerialNumber,ReleaseDate,AuthorID,GenreID,Publisher,NumberInStock,FileModelId,UploadedFile")]
        //                         CreateEditBookViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var book = unitOfWork.Books.Get(model.ID);

        //        Mapper.Map(model, book);

        //        if (model.UploadedFile != null)
        //        {
        //            book.FileModelId = FileModelHelper.CheckDbAndUploadeFile(model.UploadedFile);
        //        }


        //        unitOfWork.Save();
        //        return RedirectToAction("Index");
        //    }

        //    return View(modelBuilder.RebuildCreateEditBookViewModel(model));
        //}

        //// GET: Book/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var book = unitOfWork.Books.GetBookWithAuthor((int)id);
        //    if (book == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(book);
        //}

        //// POST: Book/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    var book = unitOfWork.Books.Get(id);
        //    unitOfWork.Books.Remove(book);
        //    unitOfWork.Save();
        //    return RedirectToAction("Index");
        //}



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
        //    ViewBag.AuthorID = new SelectList(unitOfWork.Authors.GetAuthorsByLastName(), "ID", "FullName", model.AuthorID);
        //    ViewBag.GenreID = new SelectList(unitOfWork.Genres, "ID", "Name", model.GenreID);
        //}
    }
}