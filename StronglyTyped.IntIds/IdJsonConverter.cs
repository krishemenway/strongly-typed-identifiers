using Newtonsoft.Json;
using System;

namespace StronglyTyped.IntIds
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
			writer.WriteValue(((IIntId)value).Value);
		}

		private static object ReadJsonForNonNullableId(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			var constructor = objectType.GetConstructor(new[] { typeof(int) });
			var value = serializer.Deserialize<int>(reader);
			return constructor.Invoke(new object[] { value });
		}

		private static object ReadJsonForNullableId(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			var value = serializer.Deserialize<int?>(reader);

			if (value == null)
			{
				return null;
			}

			var constructor = objectType.GenericTypeArguments[0].GetConstructor(new[] { typeof(int) });
			return constructor.Invoke(new object[] { value });
		}
	}
}
