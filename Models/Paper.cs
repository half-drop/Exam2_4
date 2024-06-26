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

    public partial class Paper
    {
        public Paper()
        {
            this.PaperDetails = new HashSet<PaperDetail>();
        }

        public int PaperID { get; set; }
        public int CourseID { get; set; }
        public string PaperName { get; set; }
        public bool PaperState { get; set; }
        public DateTime Time { get; set; }
        public int Longth { get; set; } // 注意这里是否是正确的拼写
        public double SumMark { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<PaperDetail> PaperDetails { get; set; }
    }

}
