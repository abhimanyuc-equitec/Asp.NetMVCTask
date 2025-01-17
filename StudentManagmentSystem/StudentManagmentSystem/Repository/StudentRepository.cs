using Dapper;
using StudentManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

//using static System.Net.WebRequestMethods;

namespace StudentManagmentSystem.Repository
{
    public class StudentRepository
    {
        string connection = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;

        public void InsertData(StudentModel student)
        {

            using (var con = new SqlConnection(connection))
            {
                con.Open();
                string query = "StudentInsertion";

                con.Execute(query, new
                {
                    student.Name,
                    student.RollNo,
                    student.DOB,
                    student.Gender,
                    student.Address,
                    student.PhoneNo,
                    student.DeptId
                });
            }
        }

        public StudentModel StudentInfomationView(int Id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                String query = "StudentInformationView";
                return con.QueryFirst<StudentModel>(query, new { id = @Id });
            }
        }

        public void DeleteData(int Id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "StudentDeletion";
                con.Execute(query, new {Id=@Id,Status=0});
            }
        }
        public void RestoreData(int Id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "RestoreStudent";
                con.Execute(query, new { Id = @Id, Status = 1 });
            }
        }
        public void UpdateData(StudentModel student)
        {
            using (var con = new SqlConnection(connection))
            {
                con.Open();
                string query = "StudentUpdation";

                con.Execute(query, new
                {
                    student.Name,
                    student.RollNo,
                    student.DOB,
                    student.Gender,
                    student.Address,
                    student.PhoneNo,
                    student.DeptId,
                    student.Id
                });
            }
        }
        public void Error(Exception e)
        {
            string filePath = $"C:/Users/Owner/Desktop/Asp.Net MVC/{DateTime.Now.ToString("dd-MM-yyyy")}_ErrorLog.txt";
            var message = $"{DateTime.Now}:{e}";
            File.AppendAllText(filePath, message);
        }
    }
}