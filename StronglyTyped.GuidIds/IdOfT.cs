using System;
using System.ComponentModel;

namespace StronglyTyped.GuidIds
{
	public interface IGuidId
	{
		Guid Value { get; }
	}

	[TypeConverter(typeof(IdTypeConverter))]
	public struct Id<T> : IGuidId, IEquatable<Id<T>>, IComparable<Id<T>>
	{
		public Id(Guid value)
		{
			Value = value;
		}

		public static Id<T> NewId()
		{
			return new Id<T>(Guid.NewGuid());
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

		public static explicit operator Guid(Id<T> id)
		{
			return id.Value;
		}

		public static explicit operator Id<T>(Guid id)
		{
			return new Id<T>(id);
		}

		public static explicit operator Id<T>(string id)
		{
			return new Id<T>(Guid.Parse(id));
		}

		public static bool operator ==(Id<T> a, Id<T> b)
		{
			return a.Value.Equals(b.Value);
		}

		public static bool operator !=(Id<T> a, Id<T> b)
		{
			return !a.Value.Equals(b.Value);
		}

		public Guid Value { get; set; }
	}
}
