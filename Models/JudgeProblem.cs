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
    
    public partial class JudgeProblem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JudgeProblem()
        {
            this.StudentJudgeProblems = new HashSet<StudentJudgeProblem>();
        }
    
        public int ID { get; set; }
        public int CourseID { get; set; }
        public string Title { get; set; }
        public bool Answer { get; set; }
    
        public virtual Course Course { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentJudgeProblem> StudentJudgeProblems { get; set; }
    }
}
