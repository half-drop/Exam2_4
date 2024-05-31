using Exam1_7.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exam1_7.Controllers
{
    public class UserController1 : Controller
    {
        // GET: UserController1
        private OnlineExamEntities db = new OnlineExamEntities();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Index()
        {
            var user = db.Users.ToList();
            return View(user);
        }
    }
}