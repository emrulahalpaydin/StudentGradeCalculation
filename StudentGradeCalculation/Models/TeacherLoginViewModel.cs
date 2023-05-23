using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentGradeCalculation.Models
{
    public class TeacherLoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}