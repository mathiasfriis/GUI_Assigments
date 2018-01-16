using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JavaJamAjax.Models;

namespace JavaJamAjax.Controllers
{
    public class JobsController : Controller
    {
        // GET: Jobs
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("JobsAjax");
            }
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        public ActionResult Index(JobApplicant applicant)
        {
            try
            {
                // Add the job application to a file
                var userData = "Name: " + applicant.Name + ", E-mail: " + applicant.Email +
                    ", Expirience:" + applicant.Expirience + Environment.NewLine;

                var dataFile = Server.MapPath("~/App_Data/data.txt");
                System.IO.File.AppendAllText(@dataFile, userData);

                if (Request.IsAjaxRequest())
                {
                    return PartialView("Confirmation");
                }
                return View("Confirmation");
            }
            catch
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("Error");
                }
                return View("Error");
            }
        }
    }
}