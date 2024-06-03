using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam1_7.Models
{
    public class HistoryScoreViewModel
    {
        public int StudentExamId { get; set; }
        public DateTime BegainTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string PaperName { get; set; }
        public string CourseName { get; set; }
        public int? Score { get; set; }
    }
}