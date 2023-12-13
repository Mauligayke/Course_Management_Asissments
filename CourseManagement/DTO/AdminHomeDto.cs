using CourseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagement.DTO
{
    public class AdminHomeDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<StudentDto> Students { get; set; }=new List<StudentDto>();
        public List<Course> Courses { get; set; }=new List<Course>();
        public List<TeacherDto> Teachers { get; set; } =new List<TeacherDto>();
    }
}