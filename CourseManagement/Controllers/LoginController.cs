using CourseManagement.DTO;
using CourseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace CourseManagement.Controllers
{
    
    [HandleError(ExceptionType = typeof(Exception), View = "Error")]
    public class LoginController : Controller
    {

        CourseManagementEntities db = new CourseManagementEntities();

        [HttpGet]
        public ActionResult SignIn()
        {
            return View("SignIn");
        }


        [HttpPost]
        public ActionResult SignIn(LoginDto log)
        {
            var user = (from u in db.Users.ToList()
                        where u.Email == log.Email.ToLower() && u.Password == log.Password
                        select u).FirstOrDefault();
            //ViewBag.UserId = user.Id;
            if (user == null)
            {
                ViewBag.message = "Invalid Credientials";
                return View("SignIn");
            }
            else if(user.Role.ToUpper()=="ADMIN")
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                Session["userid"] = user.Id;
                return Redirect("/Admin/AdminHome");
            }
            else if (user.Role.ToUpper() == "TEACHER")
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                Session["userid"] = user.Id;
                return Redirect("/Teacher/TeacherHome");
            } else
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                Session["userid"] = user.Id;
                return Redirect("/Student/StudentHome");
            }

        }


        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return View("SignIn");
        }
    }
}