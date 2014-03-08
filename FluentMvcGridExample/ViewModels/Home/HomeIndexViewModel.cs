using System.Collections.Generic;
using FluentMvcGridExample.Models;

namespace FluentMvcGridExample.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Person> Data { get; set; }
        public int PageIndex { get; set; }
        public int TotalCount { get; set; }
    }
}