using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Enums
{
    public class Enums
    {
        public enum UserType : byte
        {
            [Description("Öğrenci")]
            Student,
            [Description("Öğretmen")]
            Teacher,
            [Description("Admin")]
            XMaster
        }
    }
}
