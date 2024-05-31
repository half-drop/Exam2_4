using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Exam1_7.Models
{
    [MetadataType(typeof(User_Validation))]
    public partial class User
    {
        public string SecondPassword { get; set; }
    }
}

public class User_Validation
{
    [Required(ErrorMessage = "学号不能为空")]
    [StringLength(8, MinimumLength = 4, ErrorMessage = "学号长度4到8位")]
    [DisplayName("学号")]
    public string UserID { get; set; }

    [Required(ErrorMessage = "姓名不能为空")]
    [DisplayName("姓名")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "密码不能为空")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
    [DisplayName("密码")]
    public string UserPwd { get; set; }

    [Required(ErrorMessage = "身份不能为空")]
    [Range(0, 1, ErrorMessage = "身份只能是0（学生）或1（教师）")]
    [DisplayName("身份")]
    public int UserPower { get; set; }

    [Required(ErrorMessage = "班级不能为空")]
    [DisplayName("班级")]
    public string UserClass { get; set; }

    [Required(ErrorMessage = "确认密码不能为空")]
    [Compare("UserPwd", ErrorMessage = "两次密码不一致")]
    [DisplayName("确认密码")]
    public string SecondPassword { get; set; }

    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [DisplayName("邮箱")]
    public string UserEmail { get; set; }
}