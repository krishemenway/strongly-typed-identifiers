using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace StronglyTyped.IntIds
{
	public interface IIntId
	{
		int Value { get; }
	}

	[TypeConverter(typeof(IdTypeConverter))]
	[JsonConverter(typeof(IdJsonConverter))]
	public struct Id<T> : IIntId, IEquatable<Id<T>>, IComparable<Id<T>>
	{
		public Id(int value)
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

		public static explicit operator int(Id<T> id)
		{
			return id.Value;
		}

		public static explicit operator Id<T>(int id)
		{
			return new Id<T>(id);
		}

		public static explicit operator Id<T>(string id)
		{
			return new Id<T>(int.Parse(id));
		}

		public static bool operator ==(Id<T> a, Id<T> b)
		{
			return a.Value.Equals(b.Value);
		}

		public static bool operator !=(Id<T> a, Id<T> b)
		{
			return !a.Value.Equals(b.Value);
		}

		public int Value { get; set; }

		private static int LastGeneratedId { get; set; }
	}
}
