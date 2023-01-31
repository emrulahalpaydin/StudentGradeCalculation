using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
	public static class Extensions
	{
		public static string GetDescription(this Enum enumValue)
		{
			return enumValue.GetType()
					   .GetMember(enumValue.ToString())
					   .First()
					   .GetCustomAttribute<DescriptionAttribute>()?
					   .Description ?? string.Empty;
		}
	}
}
