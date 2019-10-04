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
			_idType = idType;
			_idTypeConstructor = idType.GetConstructor(new[] { typeof(string) });
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(char);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string valueAsString)
			{
				return ConvertFromStringValue(valueAsString);
			}
			else if (value is char valueAsChar)
			{
				return ConvertFromStringValue(valueAsChar.ToString());
			}

			throw new NotSupportedException($"Tried to convert from {value.GetType()} to Id<{_idType}> but not supported");
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

		private object ConvertFromStringValue(string value)
		{
			return _idTypeConstructor.Invoke(new object[] { value });
		}

		private readonly Type _idType;
		private readonly ConstructorInfo _idTypeConstructor;
	}
}
