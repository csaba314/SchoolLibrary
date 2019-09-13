using MVC.ViewModels;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Net;
using Services.Models;

namespace MVC.Controllers
{
    public class RentalsController : Controller
    {
        private IRentalServices _rentalServices;
        private IParameterBuilder _parameterBuilder;

        public RentalsController(IRentalServices rentalServices, IParameterBuilder parameterBuilder)
        {
            _rentalServices = rentalServices;
            _parameterBuilder = parameterBuilder;
        }

        // GET: /Rentals
        [HttpGet]
        public ActionResult Index(string sortOrder,
            string searchString, string currentFilter, string searchBy,
            string recordsFilter,
            int pageNumber = 1, int pageSize = 10, int id = 0)
        {

            var model = DependencyResolver.Current.GetService<RentalsIndexViewModel>();
            _parameterBuilder.Build(model, searchString: searchString, searchBy: searchBy, recordsFilter: recordsFilter, sortingParam: sortOrder,
                pageSize: pageSize, pageNumber: pageNumber, id: id, IncludeBooks: true, IncludeCustomers: true);

            model.Rentals = _rentalServices.GetAll(model.Sorting, model.Filtering, model.Paging, model.Options);

            if (id > 0)
            {
                model.Rental = Mapper.Map<RentalDTO>(_rentalServices.Get(id));
                if (model.Rental == null)
                {
                    return HttpNotFound();
                }

                TempData["model"] = model;

                return RedirectToAction("ReturnBook");
            }

            ViewBag.DateRentedSort = String.IsNullOrEmpty(sortOrder) ? "dRented" : "";
            ViewBag.DateReturnedSort = sortOrder == "dReturned" ? "dReturned_desc" : "dReturned";
            ViewBag.BookTitleSort = sortOrder == "bookTitle" ? "bookTitle_desc" : "bookTitle";
            ViewBag.CustomerNameSort = sortOrder == "customerName" ? "customerName_desc" : "customerName";

            PopulateViewBagDropDowns(model.Filtering.RecordsFilter, model.Filtering.SearchBy, model.Rentals.PageSize);
            

            return View(model);
        }

        


        //#region Create New Rental
        //// GET: /CreateRental
        //[HttpGet]
        //public ActionResult CreateRental(int customerId = 0)
        //{
        //    ViewBag.CustomerId = customerId;
        //    var model = viewModelBuilder.BuildCreateEditViewModel(customerId);

        //    if (customerId > 0)
        //    {
        //        if (model.CreateEditViewModel.Customer == null)
        //        {
        //            return HttpNotFound();
        //        }


        //        ViewBag.CustomerId = model.CreateEditViewModel.Customer.Id;
        //        ViewBag.CustomerRentalCapacity = model.CreateEditViewModel.CustomerRentalCapacity;

        //    }
        //    TempData["model"] = model;

        //    return View(model);
        //}

        //// GET /Rentals/CreateRental?bookId=1
        //[HttpGet]
        //public ActionResult AddBooks(int? bookId)
        //{
        //    var model = TempData["model"] as RentalsIndexViewModel;

        //    if (bookId != null)
        //    {
        //        var bookToAdd = unitOfWork.Books.Get((int)bookId);


        //        if ((bookToAdd != null)
        //            && (model.CreateEditViewModel.CustomerRentalCapacity > 0)
        //            && (!model.CreateEditViewModel.BooksToRent.Contains(bookToAdd)))
        //        {

        //            var newList = model.CreateEditViewModel.BooksDropdown.Where(i => i.Value != bookId.ToString());
        //            model.CreateEditViewModel.BooksDropdown = newList.OrderBy(b => b.Text);

        //            // add book to the BooksToRent list
        //            model.CreateEditViewModel.BooksToRent.Add(bookToAdd);

        //            // decrement rental capacity after a book is added
        //            model.CreateEditViewModel.CustomerRentalCapacity--;
        //        }
        //    }
        //    TempData["model"] = model;
        //    ViewBag.CustomerRentalCapacity = model.CreateEditViewModel.CustomerRentalCapacity;
        //    return View("CreateRental", model);
        //}

        //[HttpGet]
        //public ActionResult RemoveBook(int bookId)
        //{
        //    var model = TempData["model"] as RentalsIndexViewModel;

        //    var bookToRemove = unitOfWork.Books.Get(bookId);

        //    if ((bookToRemove != null)
        //        && (model.CreateEditViewModel.BooksToRent.Contains(bookToRemove)))
        //    {
        //        // remove the book from BooksToRent list
        //        model.CreateEditViewModel.BooksToRent.Remove(bookToRemove);

        //        // increment rental capacity after book is removed
        //        model.CreateEditViewModel.CustomerRentalCapacity++;

        //        // add the removed book back to the BooksDropdown list
        //        List<SelectListItem> newList = model.CreateEditViewModel.BooksDropdown.ToList();
        //        newList.Add(new SelectListItem { Text = bookToRemove.Title, Value = bookToRemove.ID.ToString() });
        //        model.CreateEditViewModel.BooksDropdown = newList.OrderBy(b => b.Text);
        //    }

        //    TempData["model"] = model;
        //    ViewBag.CustomerRentalCapacity = model.CreateEditViewModel.CustomerRentalCapacity;
        //    return View("CreateRental", model);
        //}

        //[HttpPost]
        //public ActionResult ConfirmRental()
        //{

        //    var model = TempData["model"] as RentalsIndexViewModel;

        //    foreach (var book in model.CreateEditViewModel.BooksToRent)
        //    {
        //        unitOfWork.Rentals.Add(new Rental
        //        {
        //            BookId = book.ID,
        //            CustomerId = model.CreateEditViewModel.Customer.Id,
        //            DateRented = DateTime.Now
        //        });

        //        unitOfWork.Books.Get(book.ID).RentedBooks++;
        //        unitOfWork.Customers.Get(model.CreateEditViewModel.Customer.Id).RentedBooks++;

        //    }
        //    unitOfWork.Save();

        //    return RedirectToAction("Index");
        //}
        //#endregion


        #region Return Book
        [HttpGet]
        public ActionResult ReturnBook()
        {
            var model = TempData["model"] as RentalsIndexViewModel;
            PopulateViewBagDropDowns(model.Filtering.RecordsFilter, model.Filtering.SearchBy, model.Rentals.PageSize);
            return View("Index", model);
        }

        // POST: /Rentals/ReturnBook/1
        [HttpPost]
        public ActionResult ReturnBook(RentalDTO model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rental = DependencyResolver.Current.GetService<IRental>();
            Mapper.Map(model, rental);
            _rentalServices.ReturnBook(rental);

            return RedirectToAction("Index");
        }
        #endregion

        private void PopulateViewBagDropDowns(string recordsFilter, string searchBy, int pageSize)
        {
            ViewBag.RecordsFilter = new SelectList(new List<String> { "All", "Rented", "Returned" }, recordsFilter);
            ViewBag.PageSizeDropdown = new SelectList(new List<int>() { 5, 10, 20, 40 }, pageSize);
            ViewBag.SearchByDropDown = new SelectList(new List<string> { "Book Title", "Customer Name", "Rental Id" }, searchBy);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}