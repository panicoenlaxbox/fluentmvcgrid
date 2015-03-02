using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModels.Home;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private IEnumerable<Person> ReadJsonDb()
        {
            using (var reader = new StreamReader(Server.MapPath("~/App_Data/json.txt")))
            {
                return JsonConvert.DeserializeObject<List<Person>>(reader.ReadToEnd());
            }
        }

        private IEnumerable<Person> ReadJsonDb(int numOfRecords)
        {
            var retval = ReadJsonDb();
            return numOfRecords > 0 ? retval.Take(numOfRecords) : retval;
        }

        //
        // GET: /Home/
        public ActionResult Index(int page = 1, string sort = "FirstName", string sortDir = "ASC")
        {
            var persons = ReadJsonDb();            
            switch (sort)
            {
                case "FirstName":
                    persons = sortDir == "ASC" ? persons.OrderBy(p => p.FirstName) : persons.OrderByDescending(p => p.FirstName);
                    break;
                case "LastName":
                    persons = sortDir == "ASC" ? persons.OrderBy(p => p.LastName) : persons.OrderByDescending(p => p.LastName);
                    break;
            }
            var totalCount = persons.Count();
            var pageSize = 10;
            persons = persons.Skip((page - 1) * pageSize).Take(pageSize);
            var model = new HomeIndexViewModel()
            {
                Data = persons,
                PageIndex = page,
                TotalCount = totalCount
            };
            return View(model);
        }
    }
}