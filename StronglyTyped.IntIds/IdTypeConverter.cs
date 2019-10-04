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
			_idType = idType;
			_idTypeConstructor = idType.GetConstructor(new[] { typeof(int) });
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(int) || sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string valueAsString)
			{
				return ConvertFromInt(int.Parse(valueAsString));
			}
			else if (value is int valueAsInt)
			{
				return ConvertFromInt(valueAsInt);
			}

			throw new NotSupportedException($"Tried to convert from {value.GetType()} to Id<{_idType}> but not supported");
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

		private object ConvertFromInt(int value)
		{
			return _idTypeConstructor.Invoke(new object[] { value });
		}

		private readonly Type _idType;
		private readonly ConstructorInfo _idTypeConstructor;
	}
}
