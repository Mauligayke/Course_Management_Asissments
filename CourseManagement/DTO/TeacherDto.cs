using CourseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagement.DTO
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string CourseName { get; set; }
    }
}