using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentMvcGridExample.Models;

namespace FluentMvcGridExample.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var model = new List<Person>
            {
                new Person
                {
                    FirstName = "Sergio",
                    LastName = "León",
                    Age = 38,
                    Gender = Gender.Male,
                    Birthday = new DateTime(1976, 2, 11),
                },
                new Person
                {
                    FirstName = "Carmen",
                    LastName = "Olivas",
                    Age = 37,
                    Gender = Gender.Female,
                    Birthday = new DateTime(1976, 9, 13),
                }
            };
            return View(model);
        }
	}
}