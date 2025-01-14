using Dapper;
using StudentManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentManagmentSystem.Repository
{
    public class StudentDB
    {
        String connection = ConfigurationManager.ConnectionStrings["connect"].ConnectionString;
        List<StudentModel> students = new List<StudentModel>();
      
        public void InsertData(StudentModel student)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                
                string query = " Insert into StudentInfo(Name,RollNo,DOB,Gender,Address,PhoneNo,DeptId) Values(@Name,@RollNo,@DOB,@Gender,@Address,@PhoneNo,@DeptId)";
                
                con.Execute(query,student);
            }
        }

        public StudentModel ViewData(int Id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                String query = "Select* from StudentInfo left join departmentTable  on StudentInfo.DeptId=DepartmentTable.DeptId where Id=@id";
                return con.QueryFirst<StudentModel>(query, new { id = @Id });
            }
        }

        public void DeleteData(int Id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "Update StudentInfo Set status = 0 where Id =@id";
                con.Execute(query, new {id=@Id});
            }
        }
        public void RestoreData(int Id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "Update StudentInfo Set status = 1 where Id =@id";
                con.Execute(query, new { id = @Id });
            }
        }
        public void UpdateData(StudentModel std)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "update studentInfo SET Name = @Name, RollNo = @RollNo,Gender = @Gender, Address = @Address, PhoneNo = @PhoneNo ,DeptId = @DeptId WHERE Id = @Id";
                con.Execute(query,std);
            }
        }

    }
}