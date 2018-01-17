using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Web_test.Models
{
    [Serializable]
    public class Students
    {
        public List<Student> students = new List<Student>
        {
        };

        public void setStudentList(List<Student> s)
        {
            students = s;
        }
    }
}
