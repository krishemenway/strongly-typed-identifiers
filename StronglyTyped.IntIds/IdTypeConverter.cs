using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace StronglyTyped.IntIds
{
	public class IdTypeConverter : TypeConverter
	{
		public IdTypeConverter(Type idType)
		{
			_idTypeConstructor = idType.GetConstructor(new[] { typeof(int) });
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(int) || sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			int convertedValue;

			if (value is string valueAsString)
			{
				convertedValue = int.Parse(valueAsString);
			}
			else
			{
				convertedValue = (int)value;
			}

			return _idTypeConstructor.Invoke(new object[] { convertedValue });
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(int);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return ((IIntId)value).Value.ToString();
			}

			if (destinationType == typeof(int))
			{
				return ((IIntId)value).Value;
			}

			throw new NotSupportedException($"Cannot convert to type {destinationType}");
		}

		private readonly ConstructorInfo _idTypeConstructor;
	}
}
