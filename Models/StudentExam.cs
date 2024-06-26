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
    
    public partial class StudentExam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentExam()
        {
            this.StudentFillBlankProblems = new HashSet<StudentFillBlankProblem>();
            this.StudentJudgeProblems = new HashSet<StudentJudgeProblem>();
            this.StudentMultiProblems = new HashSet<StudentMultiProblem>();
            this.StudentSingles = new HashSet<StudentSingle>();
        }
    
        public int StudentExamId { get; set; }
        public string UserID { get; set; }
        public int CourseID { get; set; }
        public int PaperID { get; set; }
        public System.DateTime BegainTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string ExamState { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentFillBlankProblem> StudentFillBlankProblems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentJudgeProblem> StudentJudgeProblems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentMultiProblem> StudentMultiProblems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentSingle> StudentSingles { get; set; }
    }
}
