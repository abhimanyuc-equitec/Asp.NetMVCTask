using StudentManagmentSystem.Models;
using StudentManagmentSystem.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using System.Drawing.Printing;
using System.Web.UI;
using System.Data;
using System.Drawing;

namespace StudentManagmentSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        StudentDB DB = new StudentDB();
        string connection = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        public ActionResult Index(int page = 1, int pageLenght = 5)
        {
            //SR NO COUNT
            int startingCount = (page - 1) * pageLenght + 1;
            ViewBag.StartingCount = startingCount;

           
            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Page", page);
                parameters.Add("@PageLenght", pageLenght);
                
                using (var query = conn.QueryMultiple("Pagination", parameters, commandType: CommandType.StoredProcedure))
                {
                    var students = query.Read<StudentModel>().ToList();
                    var totalRecords = query.Read<int>().FirstOrDefault();

                    ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageLenght);
                    ViewBag.CurrentPage = page;

                    return View(students);
                }
            }
        }
        public ActionResult Show(int page = 1, int pageLenght = 5)
        {
            //return View(DB.GetData());
            

            int startingCount = (page - 1) * pageLenght + 1;
            ViewBag.StartingCount = startingCount;

            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Page", page);
                parameters.Add("@PageLenght", pageLenght);

                using (var query = conn.QueryMultiple("PaginationShow", parameters, commandType: CommandType.StoredProcedure))
                {
                    var students = query.Read<StudentModel>().ToList();
                    var totalRecords = query.Read<int>().FirstOrDefault();

                    ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageLenght);
                    ViewBag.CurrentPage = page;

                    return View(students);
                }
            }

        }
        public ActionResult DeletedData(int page = 1, int pageLenght = 5)
        {

            int startingCount = (page - 1) * pageLenght + 1;
            ViewBag.StartingCount = startingCount;


            using (var conn = new SqlConnection(connection))
            {
                conn.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@Page", page);
                parameters.Add("@PageLenght", pageLenght);

                using (var query = conn.QueryMultiple("PaginationDeleted", parameters, commandType: CommandType.StoredProcedure))
                {
                    var students = query.Read<StudentModel>().ToList();
                    var totalRecords = query.Read<int>().FirstOrDefault();

                    ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageLenght);
                    ViewBag.CurrentPage = page;

                    return View(students);
                }
            }






            //return View(DB.DeletedData());
        }
        
        public ActionResult ViewData(int id)
        {
            return View(DB.ViewData(id));
        }
        public ActionResult ViewDataDeleted(int id)
        {
            return View(DB.ViewData(id));
        }


        public ActionResult InsertData( )
        {
            string connection = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
            using (var con = new SqlConnection(connection))
            {
                string sql = "select * from DepartmentTable";
                var id = con.Query < StudentModel>(sql).ToList();
                ViewBag.dept = id;
            }
            return View();
        }
        [HttpPost]
        public ActionResult InsertData(StudentModel student)
        {
            DB.InsertData(student);
            TempData["Inserted"] = "Student Data Created successfully!";
            return RedirectToAction("Index");
        }
        public ActionResult DeleteData(int id)
        {

            DB.DeleteData(id);
            TempData["Deleted"] = "Data deleted successfully!";
            return RedirectToAction("Index");
        }
        public ActionResult RestoreData(int id)
        {
            DB.RestoreData(id);
            TempData["Restored"] = "Data restored successfully!";
            return RedirectToAction("Index");
        }
       
        public ActionResult Edit(int Id)
        {
            using (var con = new SqlConnection(connection))
            {
                string sql = "select * from DepartmentTable";
                var ex = con.Query<StudentModel>(sql).ToList();
                ViewBag.dept = ex;
            }
            ViewBag.Id = Id;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(StudentModel std)
        {     
                DB.UpdateData(std);
                TempData["Updated"] = "Data Updated successfully!";
                return RedirectToAction("Index");
                
        }
       
        public ActionResult GetById(int Id)
        {
         
            var student = DB.ViewData(Id);
            return Json(student, JsonRequestBehavior.AllowGet); 
        }
    }
}