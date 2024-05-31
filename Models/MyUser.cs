using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam1_7.Models
{
    public class MyUser
    {
        private OnlineExamEntities db = new OnlineExamEntities();

        public IQueryable<User> GetUsersByClassName()
        {
            var us = from u in db.Users
                     where u.Classname == "信息计算1902"
                     select u;
            return us;
        }
    }
}