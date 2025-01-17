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
using System.Web.ModelBinding;

namespace StudentManagmentSystem.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        StudentRepository studentRepository = new StudentRepository();
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
                
                using (var query = conn.QueryMultiple("Pagination", parameters))
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

                using (var query = conn.QueryMultiple("PaginationDeleted", parameters))
                {
                    var students = query.Read<StudentModel>().ToList();
                    var totalRecords = query.Read<int>().FirstOrDefault();

                    ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageLenght);
                    ViewBag.CurrentPage = page;

                    return View(students);
                }

            }
        }
        
        public ActionResult ViewData(int Id)
        {
            return View(studentRepository.StudentInfomationView(Id));
        }
        public ActionResult ViewDataDeleted(int Id)
        {
            return View(studentRepository.StudentInfomationView(Id));
        }


        public ActionResult InsertData( )
        {
            
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
            using (var con = new SqlConnection(connection))
            {
                string sql = "select * from DepartmentTable";
                var id = con.Query<StudentModel>(sql).ToList();
                ViewBag.dept = id;
            }
            try
            {
                if (ModelState.IsValid && student != null)
                {
                    using (SqlConnection con = new SqlConnection(connection))
                    {
                        con.Open(); 
                        string query = "check_RollNo_"; 
                        var checkExitingRollNo = con.QuerySingleOrDefault<int>(
                            query,
                            new {student.RollNo }       
                        );
                        if (checkExitingRollNo > 0)
                        {
                            ModelState.AddModelError("RollNo", "The Roll Number already exists.");
                            return View(student); 
                        }
                    }
                    studentRepository.InsertData(student);
                    TempData["Success"] = "Student data inserted successfully!";
                    return RedirectToAction("Index");
                }

                return View(student); 
            }
            catch (SqlException ex)
            {
                TempData["Error"] = "Database error occurred: " + ex.Message;
                return View(student); 
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred: " + ex.Message;
                return View(student); 
            }
        }

        public ActionResult DeleteData(int id)
        {

            studentRepository.DeleteData(id);
            TempData["Deleted"] = "Data deleted successfully!";
            return RedirectToAction("Index");
        }
        public ActionResult RestoreData(int id)
        {
            studentRepository.RestoreData(id);
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
        //Passing the Student infomation in Ajax getById Function
        public ActionResult GetById(int Id)
        {
            var student = studentRepository.StudentInfomationView(Id);
            return Json(student, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(StudentModel student)
        {
            try
            {
                using (var con = new SqlConnection(connection))
                {
                    string sql = "select * from DepartmentTable";
                    var ex = con.Query<StudentModel>(sql).ToList();
                    ViewBag.dept = ex;
                }
                if (student != null && ModelState.IsValid) 
                {
                    studentRepository.UpdateData(student);
                    TempData["Updated"] = "Infomation Updated successfully!";
                    return RedirectToAction("index");
                }
                return View(student);
            }
            catch(Exception ex) 
            {
                studentRepository.Error(ex);
                TempData["Updated"] = "RollNo or User ALready Exits !!!";
                return RedirectToAction("Edit");
            }
            

        }
    }
}