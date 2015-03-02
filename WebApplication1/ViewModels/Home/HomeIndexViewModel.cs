using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Person> Data { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
    }
}