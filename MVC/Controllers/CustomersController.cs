using AutoMapper;
using MVC.ViewModels;
using Services.Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class CustomersController : Controller
    {
        ICustomerServices _customerServices;
        IRentalServices _rentalServices;
        IParameterBuilder _parameterBuilder;

        public CustomersController(ICustomerServices customerServices, IParameterBuilder parameterBuilder, IRentalServices rentalServices)
        {
            _customerServices = customerServices;
            _parameterBuilder = parameterBuilder;
            _rentalServices = rentalServices;
        }

        // GET: Customer
        [HttpGet]
        public ActionResult Index(string sortOrder,
                                  string searchString, string currentFilter,
                                  bool showRentalHistory = false,
                                  int pageNumber = 1, int pageSize = 10, int id = 0)
        {
            if (searchString == null)
            {
                searchString = currentFilter;
            }


            var model = DependencyResolver.Current.GetService<CustomerIndexViewModel>();
            //IEnumerable<IRental> rentals;

            _parameterBuilder.Build(model, searchString: searchString, sortingParam: sortOrder, 
                pageSize: pageSize, pageNumber: pageNumber, 
                id: id, IncludeRentalHistory: showRentalHistory);

            IEnumerable<IRental> rentals;

            model.Customers = _customerServices.GetAll(out rentals, model.Sorting, model.Filtering, model.Paging, model.Options);
            model.Rentals = rentals;

            ViewBag.LastNameSort = String.IsNullOrEmpty(sortOrder) ? "lName_desc" : "";
            ViewBag.FirstNameSort = sortOrder == "fName" ? "fName_desc" : "fName";
            ViewBag.AccountNumberSort = sortOrder == "accountNo" ? "accountNo_desc" : "accountNo"; 

            ViewBag.PageSelectList = new SelectList(new List<int> { 5, 10, 20, 40 }, model.Customers.PageSize);

            return View(model);
        }

        // GET: Customer/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "FirstName, LastName, Address, PhoneNumber, CustomerType")]
                                    CustomerDTO model)
        {
            if (ModelState.IsValid)
            {
                model.AccountNumber = _customerServices.GenerateAccountNumber();
                model.RentedBooks = 0;

                var customer = DependencyResolver.Current.GetService<ICustomer>();
                Mapper.Map(model, customer);

                _customerServices.Add(customer);

                return RedirectToAction("Index");
            }

            return View(model);
        }


        // GET: Customer/Edit/1
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = _customerServices.Get((int)id);

            if (customer == null)
            {
                return HttpNotFound("Requested customer not found in the database.");
            }

            var model = Mapper.Map<CustomerDTO>(customer);

            return View(model);
        }

        // POST: Customer/Edit/1
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, AccountNumber, FirstName, LastName, Address, PhoneNumber, CustomerType, RentedBooks")]
                                  CustomerDTO model)
        {
            if (ModelState.IsValid)
            {
                var customer = _customerServices.Get(model.Id);

                Mapper.Map(model, customer);

                _customerServices.Update(customer);

                return RedirectToAction("index");
            }

            return View(model);
        }

        // GET: Customer/DisableCustomer/1
        [HttpGet]
        public ActionResult DisableCustomer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = _customerServices.Get((int)id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            var model = new CustomerDTO();
            Mapper.Map(customer, model);
            ViewBag.CustomerName = model.LastName + ", " + model.FirstName;

            return View(model);
        }

        // POST: Customer/DisableCustomer/1
        [HttpPost]
        public ActionResult DisableCustomer(int id)
        {

            var customer = _customerServices.Get(id);
            customer.CustomerType = CustomerType.Disabled;
            _customerServices.Update(customer);

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        
    }
}