using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StronglyTyped.LongIds
{
	public class IdJsonConverter<TModel> : JsonConverter<Id<TModel>>
	{
		public override Id<TModel> Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
		{
			var constructor = objectType.GetConstructor(new[] { typeof(long) });
			return (Id<TModel>)constructor.Invoke(new object[] { reader.GetInt64() });
		}

		public override void Write(Utf8JsonWriter writer, Id<TModel> value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value.Value);
		}
	}

	public class IdJsonConverterFactory : JsonConverterFactory
	{
		public override bool CanConvert(Type typeToConvert)
		{
			if (!typeToConvert.IsGenericType)
			{
				return false;
			}

			var type = typeToConvert;

			if (!type.IsGenericTypeDefinition)
			{
				type = type.GetGenericTypeDefinition();
			}

			return type == typeof(Id<>);
		}

		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			var keyType = typeToConvert.GenericTypeArguments[0];
			var converterType = typeof(IdJsonConverter<>).MakeGenericType(keyType);

			return (JsonConverter)Activator.CreateInstance(converterType);
		}
	}
}
