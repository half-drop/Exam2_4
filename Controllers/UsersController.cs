using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Exam1_7.Models;

namespace Exam1_7.Controllers
{
    public class UsersController : Controller
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,UserName,UserPwd,UserPower,Classname,UserEmail")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,UserPwd,UserPower,Classname,UserEmail")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Exam2_4

        public ActionResult GetUsersByClassName()
        {
            MyUser myuser = new MyUser();
            return View(myuser.GetUsersByClassName());
        }

        #endregion Exam2_4

        #region Exam2_5

        public ActionResult GetUsersByUserName()
        {
            ViewData["us"] = from u in db.Users
                             where u.Classname == "信息计算1902"
                             select new { u.UserName, classname = u.Classname };
            return View();
        }

        #endregion Exam2_5

        #region Exam2_6

        public ActionResult GetUserScore()
        {
            ViewData["us"] = from u in db.Users
                             from sc in db.Scores
                             where u.UserID == sc.UserID
                             select new { u.UserName, u.Classname, sc.CourseID, sc.Score1, sc.ExamTime };
            return View();
        }

        #endregion Exam2_6

        #region Exam2_7

        public ActionResult GetUsersCount()
        {
            ViewData["uc"] = db.Users.Count();
            return View();
        }

        #endregion Exam2_7

        #region Exam2_8

        public ActionResult GetUserAverageScore()
        {
            ViewData["aver"] = db.Scores.Select(s => s.Score1).Average();
            return View();
        }

        #endregion Exam2_8

        #region Exam2_9

        public ActionResult GetUserOrderBy()
        {
            ViewData["score"] = db.Scores.OrderBy(s => s.Score1);
            return View();
        }

        #endregion Exam2_9

        #region Exam3_10_login

        //        [Route("login")]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Content("<script>alert('账号密码不能为空');location.href='Login';</script>", "text/html");
            }

            var user = db.Users.Where(u => u.UserName == username && u.UserPwd == password).SingleOrDefault();

            if (user == null)
            {
                return Content("<script>alert('输入有误');location.href='Login';</script>", "text/html");
            }
            else
            {
                FormsAuthentication.SetAuthCookie(username, false);
                TempData["Username"] = username;
                return RedirectToAction("ComposePaper");
            }
        }

        #endregion Exam3_10_login

        #region ComposePaper

        [Authorize]
        public ActionResult ComposePaper()
        {
            var username = User.Identity.Name;
            ViewBag.UserName = User.Identity.Name;
            var model = new Models.ViewModels.SingleMultipleViewModel
            {
                Username = TempData["Username"] as string,
                Single = db.SingleProblems.OrderBy(x => Guid.NewGuid()).Take(10).ToList(),
                Multiple = db.MultiProblems.OrderBy(x => Guid.NewGuid()).Take(10).ToList()
            };

            return View(model);
        }

        #endregion ComposePaper

        #region 4.18

        public ActionResult GetUser(string userid, string username)
        {
            var user = GetUserByIdAndName(userid, username);
            if (user == null)
            {
                return HttpNotFound("用户未找到");
            }
            return View(user);
        }

        private User GetUserByIdAndName(string userid, string username)
        {
            return db.Users.FirstOrDefault(u => u.UserID == userid && u.UserName == username);
        }

        #endregion 4.18
    }
}