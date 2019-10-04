using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace StronglyTyped.GuidIds
{
	public class IdTypeConverter : TypeConverter
	{
		public IdTypeConverter(Type idType)
		{
			_idType = idType;
			_idTypeConstructor = idType.GetConstructor(new[] { typeof(Guid) });
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || sourceType == typeof(Guid);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string valueAsString)
			{
				return ConvertFromGuid(Guid.Parse(valueAsString));
			}
			else if (value is Guid valueAsGuid)
			{
				return ConvertFromGuid(valueAsGuid);
			}

			throw new NotSupportedException($"Tried to convert from {value.GetType()} to Id<{_idType}> but not supported");
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string) || destinationType == typeof(Guid);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return ((IGuidId)value).Value.ToString();
			}
			else if (destinationType == typeof(Guid))
			{
				return ((IGuidId)value).Value;
			}

			throw new NotSupportedException($"Tried to convert to {destinationType} from Id<{_idType}> but not supported");
		}

		private object ConvertFromGuid(Guid value)
		{
			return _idTypeConstructor.Invoke(new object[] { value });
		}

		private readonly Type _idType;
		private readonly ConstructorInfo _idTypeConstructor;
	}
}
