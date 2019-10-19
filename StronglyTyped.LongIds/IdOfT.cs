﻿using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace StronglyTyped.LongIds
{
	public interface ILongId
	{
		long Value { get; }
	}

	/// <summary>Represents a long identifer for the specified type</summary>
	/// <typeparam name="TModel">Type the identifier is for (e.g. Person, Team)</typeparam>
	[TypeConverter(typeof(IdTypeConverter))]
	[JsonConverter(typeof(IdJsonConverterFactory))]
	public struct Id<TModel> : ILongId, IEquatable<Id<TModel>>, IComparable<Id<TModel>>
	{
		/// <summary>Create new identifier with value</summary>
		/// <param name="value">Identifier value as long</param>
		public Id(long value)
		{
			Value = value;
		}

		/// <summary>Creates a new identifier using an incrementing static long value. Probably only want to use this for testing purposes.</summary>
		/// <returns>New identifier generated from an incrementing static long value<returns>
		public static Id<TModel> NewId()
		{
			return new Id<TModel>(++LastGeneratedId);
		}

		public override bool Equals(object otherObj)
		{
			return otherObj is Id<TModel> otherId && Value.Equals(otherId.Value);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public bool Equals(Id<TModel> other)
		{
			return Value.Equals(other.Value);
		}

		public int CompareTo(Id<TModel> other)
		{
			return Value.CompareTo(other.Value);
		}

		public static explicit operator long(Id<TModel> id)
		{
			return id.Value;
		}

		public static explicit operator Id<TModel>(long id)
		{
			return new Id<TModel>(id);
		}

		public static explicit operator Id<TModel>(string id)
		{
			return new Id<TModel>(long.Parse(id));
		}

		public static bool operator ==(Id<TModel> a, Id<TModel> b)
		{
			return a.Value.Equals(b.Value);
		}

		public static bool operator !=(Id<TModel> a, Id<TModel> b)
		{
			return !a.Value.Equals(b.Value);
		}

		public long Value { get; set; }

		private static long LastGeneratedId { get; set; }
	}
}
