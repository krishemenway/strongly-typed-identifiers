using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace StronglyTyped.LongIds
{
	public interface ILongId
	{
		long Value { get; }
	}

	[TypeConverter(typeof(IdTypeConverter))]
	[JsonConverter(typeof(IdJsonConverter))]
	public struct Id<T> : ILongId, IEquatable<Id<T>>, IComparable<Id<T>>
	{
		public Id(long value)
		{
			Value = value;
		}

		public static Id<T> NewId()
		{
			return new Id<T>(++LastGeneratedId);
		}

		public override bool Equals(object otherObj)
		{
			return otherObj is Id<T> otherId && Value.Equals(otherId.Value);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public bool Equals(Id<T> other)
		{
			return Value.Equals(other.Value);
		}

		public int CompareTo(Id<T> other)
		{
			return Value.CompareTo(other.Value);
		}

		public static explicit operator long(Id<T> id)
		{
			return id.Value;
		}

		public static explicit operator Id<T>(long id)
		{
			return new Id<T>(id);
		}

		public static explicit operator Id<T>(string id)
		{
			return new Id<T>(long.Parse(id));
		}

		public static bool operator ==(Id<T> a, Id<T> b)
		{
			return a.Value.Equals(b.Value);
		}

		public static bool operator !=(Id<T> a, Id<T> b)
		{
			return !a.Value.Equals(b.Value);
		}

		public long Value { get; set; }

		private static long LastGeneratedId { get; set; }
	}
}
