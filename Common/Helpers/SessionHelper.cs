using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Helpers
{
	public static class SessionHelper
	{
		public static void Set(string key, object obj)
		{
			HttpContext.Current.Session[key] = obj;
		}

		public static T Get<T>(string key) where T : class
		{
			if (HasSession(key))
			{
				T value = (T)Convert.ChangeType(HttpContext.Current.Session[key], typeof(T));
				return value;
			}
			else
			{
				return default(T);
			}
		}

		public static bool HasSession(string key)
		{
			return HttpContext.Current.Session[key] != null ? true : false;
		}
	}
}
