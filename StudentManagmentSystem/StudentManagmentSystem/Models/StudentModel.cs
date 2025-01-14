using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagmentSystem.Models
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RollNo must be a positive number")]
        public int RollNo { get; set; }

        [Required(ErrorMessage = "Enter Your Name")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public String Name { get; set; }

        public String DepartmentName { get; set;}

        [Required(ErrorMessage = "Please select Department")]
        public int DeptId {  get; set; }

        [Required(ErrorMessage ="DOB should be Filled")]
        public DateTime DOB {  get; set;}
        public string Age {  get; set;}
        [Required(ErrorMessage = "pls Select Gender")]
        public string Gender {  get; set;}
        [Required(ErrorMessage = "Pls enter address")]
        public String Address { get; set;}
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public long PhoneNo {  get; set;}

        
    }
}