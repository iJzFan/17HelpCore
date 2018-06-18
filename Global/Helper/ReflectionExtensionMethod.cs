using System;
using System.Reflection;

namespace HELP.GlobalFile.Global.Helper
{
	public static class ReflectionExtensionMethod
	{
		public static void SetPrivateField<T>(this T type, string fieldName, object value)
		{
			type.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(type, value);
		}

		public static void SetPrivateFieldInBase<T>(this T type, string fieldName, object value)
		{
			var baseType = type.GetType().BaseType;
			while (baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic) == null)
			{
				baseType = baseType.BaseType;
				if (baseType == typeof(object))
				{
					throw new Exception("no such filed " + fieldName + " is found");
				}
			}
			baseType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(type, value);
		}

		public static void SetProperty<T>(this T type, string propertyName, object value)
		{
			type.GetType().GetProperty(propertyName).SetValue(type, value, null);
		}

		public static void SetPropertyInBase<T>(this T type, string propertyName, object value)
		{
			var baseType = type.GetType().BaseType;
			while (!baseType.GetProperty(propertyName).CanWrite)
			{
				baseType = baseType.BaseType;
				if (baseType == typeof(object))
				{
					throw new Exception("no such property " + propertyName + " is found");
				}
			}
			baseType.GetProperty(propertyName).SetValue(type, value, null);
		}
	}
}