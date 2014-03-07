using System;

namespace FluentMvcGridExample.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
    }
}