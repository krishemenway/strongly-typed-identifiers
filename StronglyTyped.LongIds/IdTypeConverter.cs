using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace StronglyTyped.LongIds
{
	public class IdTypeConverter : TypeConverter
	{
		public IdTypeConverter(Type idType)
		{
			_idType = idType;
			_idTypeConstructor = idType.GetConstructor(new[] { typeof(long) });
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string valueAsString)
			{
				return ConvertFromLong(long.Parse(valueAsString));
			}
			else if (value is long valueAsLong)
			{
				return ConvertFromLong(valueAsLong);
			}

			throw new NotSupportedException($"Tried to convert from {value.GetType()} to Id<{_idType}> but not supported");
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(long);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return ((ILongId)value).Value.ToString();
			}

			if (destinationType == typeof(long))
			{
				return ((ILongId)value).Value;
			}

			throw new NotSupportedException($"Cannot convert to type {destinationType}");
		}

		private object ConvertFromLong(long value)
		{
			return _idTypeConstructor.Invoke(new object[] { value });
		}

		private readonly Type _idType;
		private readonly ConstructorInfo _idTypeConstructor;
	}
}
