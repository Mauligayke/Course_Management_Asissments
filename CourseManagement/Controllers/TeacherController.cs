using CourseManagement.DTO;
using CourseManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagement.Controllers
{
    [Authorize]
    [HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class TeacherController : Controller
    {
        CourseManagementEntities db = new CourseManagementEntities();
        // GET: Teacher

        [HttpGet]
        public ActionResult TeacherHome()
        {
            var students = db.Students.ToList();
            List<StudentDto> studentDtos = new List<StudentDto>();
            foreach (var student in students)
            {
                var stdo = new StudentDto
                {
                    Id = student.Id,
                    CourseName = student.Course.Name,
                    Name = student.User.Name,
                    Mobile = student.User.Mobile
                };
                studentDtos.Add(stdo);
            }
            return View("TeacherHome",studentDtos);
        }

        public ActionResult AddStudent()
        {
            var courseList = db.Courses.ToList();
            return View("AddStudent", courseList);
        }
        [HttpPost]
        public ActionResult AddStudent(AddStudentDto studentDto)
        {
            var course = db.Courses.Where(x => x.Name == studentDto.CourseName).FirstOrDefault();
            User user = new User()
            {
                Email = studentDto.Email,
                Name = studentDto.Name,
                Role = studentDto.Role,
                Password = studentDto.Password,
                Id = studentDto.Id,
                Mobile = studentDto.Mobile
            };
            db.Users.Add(user);
            db.SaveChanges();
            Student std = new Student()
            {
                User = user,
                UserId = studentDto.Id,
                Course = course,
                CourseId = course.Id,
            };
            db.Students.Add(std );
            db.SaveChanges();
            return Redirect("/Teacher/TeacherHome");
        }

        public ActionResult UpdateStudent(int id)
        {
            var student = db.Students.Where(x => x.Id == id).FirstOrDefault();
            UpdateStudentDto studentDto = new UpdateStudentDto()
            {
                Id = student.UserId,
                CourseName = student.Course.Name,
                Name = student.User.Name,
                Email = student.User.Email,
                Mobile = student.User.Mobile,
                Password = student.User.Password,
                Role = student.User.Role,
                Courses = db.Courses.ToList(),
            };
            return View("UpdateStudent", studentDto);
        }
        [HttpPost]
        public ActionResult UpdateStudent(AddStudentDto studentDto,int id)
        {
            var user1 = db.Users.Where(x=>x.Id == id).FirstOrDefault();
            var course = db.Courses.Where(x => x.Name == studentDto.CourseName).FirstOrDefault();
            user1.Email = studentDto.Email;
            user1.Mobile = studentDto.Mobile;
            user1.Password = studentDto.Password;
            user1.Role = studentDto.Role;
            user1.Name = studentDto.Name;
            
            db.Users.AddOrUpdate(user1);
            db.SaveChanges();
            var student = db.Students.Where(x=> x.UserId==user1.Id).FirstOrDefault();
            student.Course = course;
            student.User = user1;
            student.CourseId = course.Id;
            student.UserId = user1.Id;
            db.Students.AddOrUpdate(student);
            db.SaveChanges();
            return Redirect("/Teacher/TeacherHome");
        }
        public ActionResult DeleteStudent(int id)
        {

            var Student = db.Students.Where(x => x.Id == id).FirstOrDefault();
            var user = db.Users.Where(x => x.Id == Student.UserId).FirstOrDefault();
            if (Student == null || user == null)
            {
                // Course not found, return a 404 Not Found response or handle appropriately.
                return HttpNotFound();
            }
            db.Students.Remove(Student);
            db.SaveChanges();
            db.Users.Remove(user);
            db.SaveChanges();
            return Redirect("/Teacher/TeacherHome");
        }
    }
}