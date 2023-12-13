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
    [HandleError(ExceptionType = typeof(Exception),View ="Error")]
    public class AdminController : Controller
    {
        CourseManagementEntities db = new CourseManagementEntities();
        // GET: Admin
        [HttpGet]
        public ActionResult AdminHome()
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



            var teachers = db.Teachers.ToList();
            List<TeacherDto> TeacherDtos = new List<TeacherDto>();
            foreach (var teacher in teachers)
            {
                var t = new TeacherDto
                {
                    Id = teacher.Id,
                    Mobile = teacher.User.Mobile,
                    Name = teacher.User.Name,
                    CourseName = db.Teacher_Course.ToList().Where(x=>x.TeacherId==teacher.Id).FirstOrDefault().Course.Name,

                };
               
              TeacherDtos.Add(t);
            }

            List<Course> courses = db.Courses.ToList();

            var adminInfo = new AdminHomeDto
            {
                Courses = courses,
                Students = studentDtos,
                Teachers = TeacherDtos,
            };

            return View("AdminHome", adminInfo);
        }

        public ActionResult AddTeacher()
        {
            var courseList = db.Courses.ToList();
            return View("AddTeacher", courseList);
        }

        [HttpPost]
        public ActionResult AddTeacher(AddTeacherDto teacher)
        {
            var course = db.Courses.Where(x => x.Name == teacher.CourseName).FirstOrDefault();
            User user = new User()
            {
                Name = teacher.Name,
                Id = teacher.Id,
                Email = teacher.Email,
                Password = teacher.Password,
                Mobile = teacher.Mobile,
                Role = teacher.Role,
            };
            db.Users.Add(user);
            db.SaveChanges();
            Teacher t = new Teacher()
            {
                User = user,
                UserId = user.Id,
            };
            db.Teachers.Add(t);
            db.SaveChanges();

            Teacher_Course tc = new Teacher_Course()
            {
                Course = course,
                CourseId = course.Id,
                TeacherId = t.Id,
                Teacher = t

            };

            db.Teacher_Course.Add(tc);
            db.SaveChanges();

            return Redirect("/Admin/AdminHome");
        }

        public ActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCourse(Course course)
        {
            db.Courses.Add(course);
            db.SaveChanges();
            return Redirect("/Admin/AdminHome");
        }

     
        public ActionResult DeleteCourse(int id)
        {
            var course = db.Courses.Where(x=> x.Id == id).FirstOrDefault();
            if (course == null)
            {
                // Course not found, return a 404 Not Found response or handle appropriately.
                return HttpNotFound();
            }
            db.Courses.Remove(course);
            db.SaveChanges();
            return Redirect("/Admin/AdminHome");
        }

        public ActionResult DeleteTeacher(int id)
        {
       
            var teacher = db.Teachers.Where(x=> x.Id == id).FirstOrDefault();
            var user = db.Users.Where(x=>x.Id == teacher.UserId).FirstOrDefault();
            if (teacher == null || user == null)
            {
                // Course not found, return a 404 Not Found response or handle appropriately.
                return HttpNotFound();
            }
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            db.Users.Remove(user);
            db.SaveChanges();
            return Redirect("/Admin/AdminHome");
        }
        public ActionResult EditTeacher(int id)
        {
            var t = db.Teachers.Where(x => x.Id == id).FirstOrDefault();
            var teacher = db.Users.Where(x=>x.Id==t.UserId).FirstOrDefault();
            var tc = db.Teacher_Course.Where(x=>x.TeacherId == id).FirstOrDefault();
            var course = db.Courses.Where(x=>x.Id == tc.CourseId).FirstOrDefault();
            UpdateTeacherDto TeacherDto = new UpdateTeacherDto()
            {
                Id = teacher.Id,
                CourseName = course.Name,
                Name = teacher.Name,
                Email = teacher.Email,
                Mobile = teacher.Mobile,
                Password = teacher.Password,
                Role = teacher.Role,
                Courses = db.Courses.ToList(),
            };
            return View("UpdateTeacher", TeacherDto);
        }

        [HttpPost]
        public ActionResult UpdateTeacher(AddTeacherDto TeacherDto, int id)
        {
            var user1 = db.Users.Where(x => x.Id == id).FirstOrDefault();
            var course = db.Courses.Where(x => x.Name == TeacherDto.CourseName).FirstOrDefault();
            user1.Email = TeacherDto.Email;
            user1.Mobile = TeacherDto.Mobile;
            user1.Password = TeacherDto.Password;
            user1.Role = TeacherDto.Role;
            user1.Name = TeacherDto.Name;

            db.Users.AddOrUpdate(user1);
            db.SaveChanges();
            var teacher = db.Teachers.Where(x => x.UserId == user1.Id).FirstOrDefault();
            Teacher_Course tc = new Teacher_Course()
            {
                Course = course,
                Teacher = teacher,
                CourseId = course.Id,
                TeacherId = teacher.Id
            };
            db.Teacher_Course.AddOrUpdate(tc);
            db.SaveChanges();
            
            teacher.User = user1;
            teacher.Teacher_Course.Add(tc);
            teacher.UserId = user1.Id;
            db.Teachers.AddOrUpdate(teacher);
            db.SaveChanges();
            return Redirect("/Admin/AdminHome");
        }


    }
}