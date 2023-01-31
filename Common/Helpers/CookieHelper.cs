using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Helpers
{
	public class CookieHelper
	{
		public static void CreateCookie(string name, string value, DateTime? Expires)
		{
			HttpCookie cookieVisitor = new HttpCookie(name, value);
			cookieVisitor.Expires = Expires ?? DateTime.Now.AddDays(7);
			HttpContext.Current.Response.Cookies.Add(cookieVisitor);
		}
		public static string GetCookie(string name)
		{
			//Böyle bir cookie mevcut mu kontrol ediyoruz
			if (HttpContext.Current.Request.Cookies.AllKeys.Contains(name))
			{
				//böyle bir cookie varsa bize geri değeri döndürsün
				return HttpContext.Current.Request.Cookies[name].Value;
			}
			return null;
		}
		public static void DeleteCookie(string name)
		{
			//Böyle bir cookie var mı kontrol ediyoruz
			if (GetCookie(name) != null)
			{
				//Varsa cookiemizi temizliyoruz
				HttpContext.Current.Response.Cookies.Remove(name);
				//ya da 
				HttpContext.Current.Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);
			}
		}
		//Tüm Cookieleri temizler.
		public static void ClearAll(string key)
		{
			var httpContext = new HttpContextWrapper(HttpContext.Current);
			HttpCookie cookie = new HttpCookie(key)
			{
				Expires = DateTime.Now.AddDays(-1) // or any other time in the past
			};
			HttpContext.Current.Response.Cookies.Set(cookie);
		}
		/// <summary>
		/// Expires i valuese 'enddate' parametresi ile aktarır
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <param name="Expires"></param>
		public static void CreateCookieIntegration(string name, string value, DateTime? Expires)
		{
			HttpCookie cookieVisitor = new HttpCookie(name, value);
			DateTime appendTime = Expires ?? DateTime.Now.AddDays(7);

			cookieVisitor.Expires = appendTime;
			cookieVisitor.Values.Add("enddate", appendTime.ToString());
			HttpContext.Current.Response.Cookies.Add(cookieVisitor);
		}
		/// <summary>
		/// Cookienin valuesleri altından 'enddate' i çeker
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static DateTime? GetCookieExpires(string name)
		{
			if (HttpContext.Current.Request.Cookies.AllKeys.Contains(name))
			{
				HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
				var exp = cookie["enddate"];
				return Convert.ToDateTime(exp);
			}
			return null;
		}
	}

}
