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

        public float averageGrade
        {
            get
            {
                return getAverageGrade();
            }
        }

        public float getAverageGrade()
        {
            int sum = 0;
            foreach(Student s in students)
            {
                sum += s.Grade;
            }
            float avg = sum / students.Count;
            return avg;
        }
    }
}
