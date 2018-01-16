using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Web_test.Models
{
    public class Student
    {
        public string Name { get; set;}
        public int Grade { get; set; }
        public int ID { get; set; }

        public Student()
        {
            Name = "Test";
            Grade = 12;
        }
    }
}
