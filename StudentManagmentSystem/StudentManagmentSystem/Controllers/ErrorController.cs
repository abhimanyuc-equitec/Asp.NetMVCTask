using StudentManagmentSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManagmentSystem.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        StudentRepository studentRepository = new StudentRepository();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PageNotFound(Exception e)
        {
            studentRepository.Error(e);
            return View();
        }
        public ActionResult ServerError (Exception e) 
        {
            studentRepository.Error(e);
            return View();
        }
    }
}