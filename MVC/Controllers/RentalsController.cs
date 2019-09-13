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
using System.Collections;

namespace MVC.Controllers
{
    public class RentalsController : Controller
    {
        private IRentalServices _rentalServices;
        private ICustomerServices _customerServices;
        private IBookServices _bookServices;
        private IParameterBuilder _parameterBuilder;

        public RentalsController(
            IRentalServices rentalServices, 
            IParameterBuilder parameterBuilder,
            ICustomerServices customerServices,
            IBookServices bookServices)
        {
            _rentalServices = rentalServices;
            _customerServices = customerServices;
            _bookServices = bookServices;
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




        #region Create New Rental
        // GET: /CreateRental
        [HttpGet]
        public ActionResult CreateRental(int customerId = 0)
        {
            ViewBag.CustomerId = customerId;
            var model = DependencyResolver.Current.GetService<RentalsIndexViewModel>();
            model.Rental = /*DependencyResolver.Current.GetService<RentalDTO>();*/new RentalDTO();
            model.Rental.BooksToRent = new List<IBook>();
            ViewBag.CustomersDropDown = new SelectList(_rentalServices.PopulateCustomersDropDown(), "Key", "Value");

            if (customerId > 0)
            {
                model.Rental.Customer = _customerServices.Get(customerId);

                if (model.Rental.Customer == null)
                {
                    return HttpNotFound();
                }
                model.Rental.CustomerId = model.Rental.Customer.Id;
                model.Rental.CustomerRentalCapacity = _rentalServices.GetCustomerRentalCapacity(model.Rental.Customer);
                ViewBag.BooksDropDown = new SelectList(_rentalServices.PopulateBooksDropDown(), "Key", "Value");

                ViewBag.CustomerId = model.Rental.Customer.Id;
                ViewBag.CustomerRentalCapacity = model.Rental.CustomerRentalCapacity;

            }
            TempData["model"] = model;

            return View(model);
        }

        // GET /Rentals/CreateRental?bookId=1
        [HttpGet]
        public ActionResult AddBooks(int? bookId)
        {
            var model = TempData["model"] as RentalsIndexViewModel;

            if (bookId != null)
            {
                var bookToAdd = _bookServices.Get((int)bookId);


                if ((bookToAdd != null)
                    && (model.Rental.CustomerRentalCapacity > 0)
                    && (!model.Rental.BooksToRent.Contains(bookToAdd)))
                {
                    // add book to the BooksToRent list
                    model.Rental.BooksToRent.Add(bookToAdd);

                    // remove the book from the dropdown list
                    ViewBag.BooksDropDown = new SelectList(_rentalServices.PopulateBooksDropDown()
                                                            .Where(kvp => kvp.Key != bookId.ToString()), "Key", "Value");

                    // decrement customer rental capacity after a book is added
                    model.Rental.CustomerRentalCapacity--;
                }
            }
            TempData["model"] = model;
            ViewBag.CustomerRentalCapacity = model.Rental.CustomerRentalCapacity;
            return View("CreateRental", model);
        }

        [HttpGet]
        public ActionResult RemoveBook(int bookId)
        {
            var model = TempData["model"] as RentalsIndexViewModel;

            var bookToRemove = _bookServices.Get(bookId);

            if ((bookToRemove != null)
                && (model.Rental.BooksToRent.Contains(bookToRemove)))
            {
                // remove the book from BooksToRent list
                model.Rental.BooksToRent.Remove(bookToRemove);

                // increment rental capacity after book is removed
                model.Rental.CustomerRentalCapacity++;

                // add the removed book back to the BooksDropdown list

                //List<SelectListItem> newList = model.BooksDropDown.ToList();
                //newList.Add(new SelectListItem { Text = bookToRemove.Title, Value = bookToRemove.ID.ToString() });
                //model.BooksDropDown = new SelectList(newList.OrderBy(b => b.Text));
            }

            TempData["model"] = model;
            ViewBag.CustomerRentalCapacity = model.Rental.CustomerRentalCapacity;
            return View("CreateRental", model);
        }

        [HttpPost]
        public ActionResult ConfirmRental()
        {

            var model = TempData["model"] as RentalsIndexViewModel;

            foreach (var book in model.Rental.BooksToRent)
            {
                var newRental = DependencyResolver.Current.GetService<IRental>();
                newRental.BookId = book.ID;
                newRental.CustomerId = model.Rental.Customer.Id;
                newRental.DateRented = DateTime.Now;

                _rentalServices.Add(newRental);
            }
            return RedirectToAction("Index");
        }
        #endregion


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