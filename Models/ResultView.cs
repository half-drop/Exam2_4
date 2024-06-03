using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam1_7.Models
{
    public class ResultViewModel
    {
        public string Title { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class ResultPageViewModel
    {
        public int TotalScore { get; set; }
        public List<ResultViewModel> Results { get; set; }
    }
}