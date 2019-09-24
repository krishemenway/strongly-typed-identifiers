using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace StronglyTyped.StringIds
{
	public class IdTypeConverter : TypeConverter
	{
		public IdTypeConverter(Type idType)
		{
			_idTypeConstructor = idType.GetConstructor(new[] { typeof(string) });
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return _idTypeConstructor.Invoke(new object[] { (string)value });
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return ((IStringId)value).Value;
			}

			throw new NotSupportedException($"Cannot convert to type {destinationType}");
		}

		private readonly ConstructorInfo _idTypeConstructor;
	}
}
