using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam1_7.Models
{
    public class PaperViewModel
    {
        public int PaperID { get; set; }
        public bool PaperState { get; set; }
        public string PaperName { get; set; }
        public string CourseName { get; set; }
        public DateTime Time { get; set; }
        public int Longth { get; set; }
        public double SumMark { get; set; }
    }
}