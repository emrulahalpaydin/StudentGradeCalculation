using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
	public class DB:DbContext
	{
		public DB():base("name=DB")
		{
			this.Configuration.LazyLoadingEnabled = false;
			this.Configuration.ProxyCreationEnabled = false;
			System.Data.Entity.Database.SetInitializer<DB>(null);
		}
		public DbSet<Student> Students { get; set; }
	}
}
