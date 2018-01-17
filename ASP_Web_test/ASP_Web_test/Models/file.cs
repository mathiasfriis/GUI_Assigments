using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Web_test.Models
{
    public class file
    {
        public Students students { get; set; }
        public String fileName { get; set; }

        public file()
        {

        }

        public file(Students list)
        {
            students = list;
        }

        public void setStudentList(Students s)
        {
            students = s;
        }
    }
}
