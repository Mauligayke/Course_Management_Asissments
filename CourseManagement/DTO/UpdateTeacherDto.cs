using CourseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagement.DTO
{
    public class UpdateTeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }
        public string CourseName { get; set; }

        public List<Course> Courses { get; set; }
    }
}