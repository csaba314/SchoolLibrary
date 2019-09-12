using AutoMapper;
using MVC.ViewModels;
using Services.Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class AuthorsController : Controller
    {
        private IAuthorServices _services;
        private IBookServices _bookServices;
        private IFileServices _fileServices;
        private IParameterBuilder _parameterBuilder;

        public AuthorsController(IAuthorServices services,
                                 IBookServices bookServices,
                                 IFileServices fileServices, 
                                 IParameterBuilder parameterBuilder)
        {
            _services = services;
            _bookServices = bookServices;
            _fileServices = fileServices;
            _parameterBuilder = parameterBuilder;
        }

        // GET: Authors
        public ActionResult Index(int? id, int? bookId, string sortOrder, string searchString, string currentFilter, int pageSize = 10, int pageNumber = 1)
        {

            if (searchString == null)
            {
                searchString = currentFilter;
            }
            else
            {
                pageNumber = 1;
            }

            var model = DependencyResolver.Current.GetService<AuthorIndexViewModel>();
            //var model = new AuthorIndexViewModel();

            _parameterBuilder.Build(model, searchString, sortOrder, pageSize, pageNumber);
            model.Authors = _services.GetAll(model.Sorting, model.Filtering, model.Paging, model.Options);

            if (id != null)
            {
                model.Id = id.Value;
                model.Books = _bookServices.GetBooksByAuthor(model.Id);
            }

            if (bookId != null)
            {
                ViewBag.BookId = bookId;
            }

            ViewBag.LastNameSort = String.IsNullOrEmpty(sortOrder) ? "lName_desc" : "";
            ViewBag.FirstNameSort = sortOrder == "fName" ? "fName_desc" : "fName";
                        

            return View(model);
        }


        // GET: Authors/Create
        public ActionResult Create()
        {
            var model = DependencyResolver.Current.GetService<AuthorDTO>();
            //var model = new AuthorDTO();
            return View(model);
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstName,DateOfBirth,DateOfDeath,AboutAuthor,UploadedFile")] AuthorDTO model)
        {

            if (ModelState.IsValid)
            {

                var author = DependencyResolver.Current.GetService<IAuthor>();
                //var author = new Author();
                Mapper.Map(model, author);

                if (model.UploadedFile != null)
                {
                    author.FileModelId = _fileServices.CheckDbAndUploadeFile(model.UploadedFile);
                }

                _services.Add(author);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var author = _services.GetAuthorWithBooks((int)id);
            if (author == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<AuthorDTO>(author);

            if (author.FileModelId != null)
            {
                model.FileModel = _fileServices.Get((int) author.FileModelId);
            }

            return View(model);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,DateOfBirth,DateOfDeath,AboutAuthor,FileModelId,UploadedFile")] AuthorDTO model)
        {
            if (ModelState.IsValid)
            {
                var author = _services.GetAuthorWithBooks(model.ID);

                Mapper.Map(model, author);

                if (model.UploadedFile != null)
                {
                    author.FileModelId = _fileServices.CheckDbAndUploadeFile(model.UploadedFile);
                }

                _services.Update(author);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = _services.GetAuthorWithBooks((int)id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<AuthorDTO>(author));
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var author = _services.GetAuthorWithBooks(id);
            _services.Delete(author);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}