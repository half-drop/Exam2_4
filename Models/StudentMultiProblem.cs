//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exam1_7.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentMultiProblem
    {
        public int StudentMultiProblemID { get; set; }
        public int StudentExamId { get; set; }
        public int MultiProblemID { get; set; }
        public string answer { get; set; }
    
        public virtual MultiProblem MultiProblem { get; set; }
        public virtual StudentExam StudentExam { get; set; }
    }
}
