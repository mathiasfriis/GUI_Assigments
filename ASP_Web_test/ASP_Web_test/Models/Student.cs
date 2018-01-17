using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Web_test.Models
{
    [Serializable]
    public class Student
    {
        public string Name { get; set;}
        public int Quantity { get; set; }
        public int ID { get; set; }

        public Student()
        {
            Name = "Test";
            Quantity = 12;
        }
    }
}
