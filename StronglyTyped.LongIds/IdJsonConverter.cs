using Newtonsoft.Json;
using System;

namespace StronglyTyped.LongIds
{
	public class IdJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return Nullable.GetUnderlyingType(objectType) == null
				? ReadJsonForNonNullableId(reader, objectType, serializer)
				: ReadJsonForNullableId(reader, objectType, serializer);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(((ILongId)value).Value);
		}

		private static object ReadJsonForNonNullableId(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			var constructor = objectType.GetConstructor(new[] { typeof(long) });
			var value = serializer.Deserialize<long>(reader);
			return constructor.Invoke(new object[] { value });
		}

		private static object ReadJsonForNullableId(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			var value = serializer.Deserialize<long?>(reader);

			if (value == null)
			{
				return null;
			}

			var constructor = objectType.GenericTypeArguments[0].GetConstructor(new[] { typeof(long) });
			return constructor.Invoke(new object[] { value });
		}
	}
}
