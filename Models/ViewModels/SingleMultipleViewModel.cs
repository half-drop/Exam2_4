using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exam1_7.Models.ViewModels
{
    public class SingleMultipleViewModel
    {
        public string Username { get; set; }
        public IEnumerable<SingleProblem> Single { get; set; }
        public IEnumerable<MultiProblem> Multiple { get; set; }
    }
}