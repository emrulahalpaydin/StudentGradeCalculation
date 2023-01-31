using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
	public class Student
	{
		[Key]
		public long ID { get; set; }
		public string NameSurname { get; set; }
		public string Password { get; set; }
		public long StudentNumber { get; set; }
		public bool Active { get; set; }
		public DateTime CreateDate { get; set; } = DateTime.Now;
		public DateTime LastLoginDate { get; set; } = DateTime.Now;
	}
}
