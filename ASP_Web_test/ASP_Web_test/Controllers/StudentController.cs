using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP_Web_test.Models;
using ASP_Web_test.Utils;

namespace ASP_Web_test.Controllers
{
    public class StudentController : Controller
    {
        SerialCommunicator serialCommunicator = new SerialCommunicator();
        Students students = new Students();
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        // GET: Student/Details/5
        public ActionResult Details()
        {
            readStudentsFromFile();
            return View("Details", students);
        }

        // GET: Student/SaveAs
        public ActionResult SaveAs()
        {
            return View("SaveAs", new file(students));
        }

        // POST: Student/SaveAs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAs(file myFile)
        {
            try
            {
                //students=myFile.students;
                saveToFile(myFile.fileName);
                return View("Details", students);
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Open
        public ActionResult OpenFromFile()
        {
            return View("OpenFromFile", new file(students));
        }

        // POST: Student/SaveAs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenFromFile(file myFile)
        {
            try
            {
                readFromFile(myFile.fileName);
                readStudentsFromFile();
                return View("Details", students);
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        public ActionResult Create(Student student)
        {
            try
            {
                // TODO: Add insert logic here
                readStudentsFromFile();
                students.students.Add(student);
                writeStudentsToFile();
                return View("Details", students);
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(string name)
        {
            readStudentsFromFile();
            var st = students.students.Find(s => s.Name == name);
            return View("Edit", st);
        }
        

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            try
            {
                readStudentsFromFile();
                int index = students.students.FindIndex(s => s.Name == student.Name);
                students.students[index] = student;
                writeStudentsToFile();
                return View("Details", students);
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(string name)
        {
            readStudentsFromFile();
            var st = students.students.Find(s => s.Name == name);
            return View("Delete",st);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Student student)
        {
            try
            {
                readStudentsFromFile();
                Student st = students.students.Find(s => s.Name == student.Name);
                students.students.Remove(st);
                writeStudentsToFile();
                return View("Details", students);
            }
            catch
            {
                return View();
            }
        }

        public void saveToFile(string filename)
        {
            serialCommunicator.SerializeObject(students, filename);
        }

        public void readFromFile(string filename)
        {
            students = serialCommunicator.DeSerializeObject<Students>(filename);
        }

        public void writeStudentsToFile()
        {
            //write to File
            serialCommunicator.SerializeObject(students, "myStudents");
        }

        public void readStudentsFromFile()
        {
            //read from File
            students = serialCommunicator.DeSerializeObject<Students>("myStudents");
        }

        public List<Student> getStudentListFromFile(string filename)
        {
            List<Student> students = new List<Student>();
            Student s = new Student();
            s.Name = "Student read from file";
            s.Quantity = 4;
            students.Add(s);
            return students;
        }
    }
}