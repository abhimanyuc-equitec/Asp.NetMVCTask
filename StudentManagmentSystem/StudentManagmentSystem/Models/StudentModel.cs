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

        [Required]
        [Range(1, int.MaxValue,ErrorMessage ="Enter positive integer" )]
        [RegularExpression(@"\d+"/*, ErrorMessage = "RollNo must be a whole number without decimal points"*/)]
        [Key]
        public int RollNo { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public String Name { get; set; }

        public String DepartmentName { get; set;}

        [Required]
        public int DeptId {  get; set; }

        [Required]
        public DateTime DOB {  get; set;}
        public string Age {  get; set;}
        [Required]
        public string Gender {  get; set;}
        [Required]
        public String Address { get; set;}
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone No Must be 10 Digit positive")]
        public long PhoneNo {  get; set;}

    }
}