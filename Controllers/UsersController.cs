using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
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

        #region Exam3_10_login_simplified

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
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
                TempData["UserPower"] = user.UserPower;
                // 使用统一的Dashboard视图，通过用户权限区分显示内容
                return RedirectToAction("Dashboard");
            }
        }

        #endregion Exam3_10_login_simplified

        #region Dashboard

        [Authorize]
        public ActionResult Dashboard()
        {
            var username = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return HttpNotFound("用户未找到");
            }

            ViewBag.UserPower = user.UserPower;

            var availablePapers = db.Papers.Where(p => p.PaperState).ToList();
            if (!availablePapers.Any())
            {
                ViewBag.RandomPaperId = 0; // 或处理无有效试卷的情况
            }
            else
            {
                Random rnd = new Random();
                int index = rnd.Next(availablePapers.Count); // 随机选择一个索引
                ViewBag.RandomPaperId = availablePapers[index].PaperID; // 随机试卷ID
            }

            return View(user);
        }

        #endregion Dashboard

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

        [HttpPost]
        public ActionResult SubmitPaper(FormCollection answers)
        {
            int score = 0;

            foreach (var key in answers.AllKeys.Where(k => k.StartsWith("Single_")))
            {
                var questionId = int.Parse(key.Split('_')[1]);
                var userAnswer = answers[key];
                var correctAnswer = db.SingleProblems.Find(questionId)?.Answer;
                if (userAnswer == correctAnswer)
                {
                    score += 1;
                }
            }

            foreach (var key in answers.AllKeys.Where(k => k.StartsWith("Multi_")))
            {
                var questionId = int.Parse(key.Split('_')[1]);
                var userAnswers = answers.GetValues(key);
                var correctAnswer = db.MultiProblems.Find(questionId)?.Answer;

                var userAnswerConcat = string.Join("", userAnswers.OrderBy(c => c));
                if (userAnswerConcat == correctAnswer)
                {
                    score += 1;
                }
            }

            ViewBag.Score = score;
            return View("Result", ViewBag);
        }

        #region AdminComposePaper

        // GET: AdminComposePaper
        public ActionResult AdminComposePaper()
        {
            return View();
        }

        // POST: AdminComposePaper
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminComposePaper(string paperName, int judgeCount, int judgeMark, int singleCount, int singleMark, int multiCount, int multiMark, int fillBlankCount, int fillBlankMark)
        {
            if (string.IsNullOrEmpty(paperName))
            {
                ViewBag.Error = "试卷名称不能为空";
                return View();
            }

            // 创建试卷实体并保存
            var paper = new Paper
            {
                PaperName = paperName,
                CourseID = 1,
                PaperState = true,
                Time = DateTime.Now,
                Longth = judgeCount + singleCount + multiCount + fillBlankCount,
                SumMark = judgeCount * judgeMark + singleCount * singleMark + multiCount * multiMark + fillBlankCount * fillBlankMark
            };
            db.Papers.Add(paper);
            db.SaveChanges();

            // 随机选择题目
            var judgeProblems = db.JudgeProblems.OrderBy(j => Guid.NewGuid()).Take(judgeCount).ToList();
            var singleProblems = db.SingleProblems.OrderBy(s => Guid.NewGuid()).Take(singleCount).ToList();
            var multiProblems = db.MultiProblems.OrderBy(m => Guid.NewGuid()).Take(multiCount).ToList();
            var fillBlankProblems = db.FillBlankProblems.OrderBy(f => Guid.NewGuid()).Take(fillBlankCount).ToList();

            // 保存选择的题目到PaperDetail
            foreach (var problem in judgeProblems)
            {
                db.PaperDetails.Add(new PaperDetail { PaperID = paper.PaperID, TitleID = problem.ID, Type = "Judge", Mark = judgeMark });
            }
            foreach (var problem in singleProblems)
            {
                db.PaperDetails.Add(new PaperDetail { PaperID = paper.PaperID, TitleID = problem.ID, Type = "Single", Mark = singleMark });
            }
            foreach (var problem in multiProblems)
            {
                db.PaperDetails.Add(new PaperDetail { PaperID = paper.PaperID, TitleID = problem.ID, Type = "Multi", Mark = multiMark });
            }
            foreach (var problem in fillBlankProblems)
            {
                db.PaperDetails.Add(new PaperDetail { PaperID = paper.PaperID, TitleID = problem.ID, Type = "FillBlank", Mark = fillBlankMark });
            }

            db.SaveChanges();

            return RedirectToAction("TakeExam", new { paperId = paper.PaperID });
        }

        #endregion AdminComposePaper

        #region TakeExam

        public ActionResult TakeExam(int paperId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var paper = db.Papers.Include(p => p.PaperDetails).FirstOrDefault(p => p.PaperID == paperId);
            if (paper == null)
            {
                return HttpNotFound("未找到指定的试卷");
            }

            // 创建或更新学生的考试记录
            var studentExam = new StudentExam
            {
                UserID = user.UserID,
                CourseID = paper.CourseID,
                PaperID = paperId,
                BegainTime = DateTime.Now,
                ExamState = "Started"
            };

            db.StudentExams.Add(studentExam);
            db.SaveChanges();

            // 存储考试ID以便提交答案时使用
            TempData["StudentExamId"] = studentExam.StudentExamId;

            // 加载各种题型
            ViewData["JudgeProblems"] = paper.PaperDetails.Where(pd => pd.Type == "Judge")
                                                          .Select(pd => db.JudgeProblems.Find(pd.TitleID)).ToList();
            ViewData["SingleProblems"] = paper.PaperDetails.Where(pd => pd.Type == "Single")
                                                           .Select(pd => db.SingleProblems.Find(pd.TitleID)).ToList();
            ViewData["MultiProblems"] = paper.PaperDetails.Where(pd => pd.Type == "Multi")
                                                          .Select(pd => db.MultiProblems.Find(pd.TitleID)).ToList();
            ViewData["FillBlankProblems"] = paper.PaperDetails.Where(pd => pd.Type == "FillBlank")
                                                              .Select(pd => db.FillBlankProblems.Find(pd.TitleID)).ToList();

            return View(paper);
        }

        #endregion TakeExam

        #region SubmitAnswers

        [HttpPost]
        public ActionResult SubmitAnswers(int paperId, FormCollection answers)
        {
            var paper = db.Papers.Include(p => p.PaperDetails).FirstOrDefault(p => p.PaperID == paperId);
            if (paper == null)
            {
                return HttpNotFound("未找到指定的试卷");
            }

            int studentExamId = (int)TempData["StudentExamId"];
            var studentExam = db.StudentExams.Find(studentExamId);
            if (studentExam == null)
            {
                return HttpNotFound("未找到学生考试记录");
            }

            int totalScore = 0;

            // 判断题处理
            foreach (var detail in paper.PaperDetails.Where(pd => pd.Type == "Judge"))
            {
                var problem = db.JudgeProblems.Find(detail.TitleID);
                var userAnswer = answers["Judge_" + problem.ID];
                bool isCorrect = (userAnswer == "true" && problem.Answer) || (userAnswer == "false" && !problem.Answer);

                db.StudentJudgeProblems.Add(new StudentJudgeProblem
                {
                    StudentExamId = studentExam.StudentExamId,
                    JudgeProblemID = problem.ID,
                    answer = userAnswer == "true"
                });

                totalScore += isCorrect ? detail.Mark : 0;
            }

            // 单选题处理
            foreach (var detail in paper.PaperDetails.Where(pd => pd.Type == "Single"))
            {
                var problem = db.SingleProblems.Find(detail.TitleID);
                var userAnswer = answers["Single_" + problem.ID];
                bool isCorrect = userAnswer == problem.Answer;

                db.StudentSingles.Add(new StudentSingle
                {
                    StudentExamId = studentExam.StudentExamId,
                    SingleProblemID = problem.ID,
                    answer = userAnswer
                });

                totalScore += isCorrect ? detail.Mark : 0;
            }

            // 多选题处理
            foreach (var detail in paper.PaperDetails.Where(pd => pd.Type == "Multi"))
            {
                var problem = db.MultiProblems.Find(detail.TitleID);
                var userAnswers = answers.AllKeys
                        .Where(k => k.StartsWith("Multi_" + problem.ID + "["))
                        .Select(k => answers[k])
                        .OrderBy(a => a);

                var userAnswerString = userAnswers.Any() ? string.Join(",", userAnswers) : "";

                // 直接从数据库获取并处理答案字符串
                var correctAnswerString = problem.Answer
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .OrderBy(a => a)
                    .Aggregate((current, next) => current + "," + next);

                // 直接比较字符串判断答案是否正确
                bool isCorrect = userAnswerString.Equals(correctAnswerString);

                db.StudentMultiProblems.Add(new StudentMultiProblem
                {
                    StudentExamId = studentExam.StudentExamId,
                    MultiProblemID = problem.ID,
                    answer = string.Join("", userAnswers)
                });

                totalScore += isCorrect ? detail.Mark : 0;
            }

            // 填空题处理
            foreach (var detail in paper.PaperDetails.Where(pd => pd.Type == "FillBlank"))
            {
                var problem = db.FillBlankProblems.Find(detail.TitleID);
                var userAnswer = answers["FillBlank_" + problem.ID];
                bool isCorrect = userAnswer == problem.Answer;

                db.StudentFillBlankProblems.Add(new StudentFillBlankProblem
                {
                    StudentExamId = studentExam.StudentExamId,
                    FillBlankProblemID = problem.ID,
                    answer = userAnswer
                });

                totalScore += isCorrect ? detail.Mark : 0;
            }

            // 更新学生考试记录的结束时间和总分
            studentExam.EndTime = DateTime.Now;
            studentExam.ExamState = "Completed";
            db.SaveChanges();

            // 将分数保存到Score表
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var score = new Score
            {
                UserID = user.UserID,
                PaperID = paperId,
                CourseID = paper.CourseID,
                Score1 = totalScore,
                ExamTime = (DateTime)studentExam.EndTime
            };
            db.Scores.Add(score);
            db.SaveChanges();

            // 重定向到结果页面
            return RedirectToAction("ResultPage", new { studentExamId = studentExam.StudentExamId });
        }

        #endregion SubmitAnswers

        public ActionResult ResultPage(int studentExamId)
        {
            var studentExam = db.StudentExams
                .Include(se => se.StudentJudgeProblems.Select(jp => jp.JudgeProblem))
                .Include(se => se.StudentSingles.Select(sp => sp.SingleProblem))
                .Include(se => se.StudentMultiProblems.Select(mp => mp.MultiProblem))
                .Include(se => se.StudentFillBlankProblems.Select(fp => fp.FillBlankProblem))
                .FirstOrDefault(se => se.StudentExamId == studentExamId);

            if (studentExam == null)
            {
                return HttpNotFound("未找到考试记录");
            }

            var totalScore = db.Scores.FirstOrDefault(s => s.UserID == studentExam.UserID && s.PaperID == studentExam.PaperID && s.ExamTime == studentExam.EndTime)?.Score1;

            ViewBag.TotalScore = totalScore;

            return View(studentExam);
        }

        public ActionResult HistoryScores()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            string userId = user.UserID;

            var historyScores = (from se in db.StudentExams
                                 join p in db.Papers on se.PaperID equals p.PaperID
                                 join c in db.Courses on p.CourseID equals c.ID
                                 where se.UserID == userId
                                 select new
                                 {
                                     se.StudentExamId,
                                     se.BegainTime,
                                     se.EndTime,
                                     p.PaperName,
                                     CourseName = c.Name,
                                     Score = (from s in db.Scores
                                              where s.UserID == se.UserID && s.PaperID == se.PaperID && s.ExamTime == se.EndTime
                                              select s.Score1).FirstOrDefault()
                                 }).ToList()
                                 .Select(exam => new HistoryScoreViewModel
                                 {
                                     StudentExamId = exam.StudentExamId,
                                     BegainTime = exam.BegainTime,
                                     EndTime = exam.EndTime,
                                     PaperName = exam.PaperName,
                                     CourseName = exam.CourseName,
                                     Score = exam.Score
                                 }).ToList();

            return View(historyScores);
        }
    }
}