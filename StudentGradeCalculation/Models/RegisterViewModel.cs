using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentGradeCalculation.Models
{
	public class RegisterViewModel
	{
		public string NameSurname { get; set; }
		public long StudentNumber { get; set; }
		public string Password { get; set; }
		public string PasswordConfirmation { get; set; }
	}
}