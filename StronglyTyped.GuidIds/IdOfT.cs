using System;
using System.ComponentModel;

namespace StronglyTyped.GuidIds
{
	public interface IGuidId
	{
		Guid Value { get; }
	}

	/// <summary>Represents a Guid identifer for the specified type</summary>
	/// <typeparam name="TModel">Type the identifier is for (e.g. Person, Team)</typeparam>
	[TypeConverter(typeof(IdTypeConverter))]
	public struct Id<TModel> : IGuidId, IEquatable<Id<TModel>>, IComparable<Id<TModel>>
	{
		/// <summary>Create new identifier with value</summary>
		/// <param name="value">Identifier value as Guid</param>
		public Id(Guid value)
		{
			Value = value;
		}

		/// <summary>Creates a new identifier using Guid.NewGuid. Probably only want to use this for testing purposes.</summary>
		/// <returns>New identifier generated from Guid.NewGuid()</returns>
		public static Id<TModel> NewId()
		{
			return new Id<TModel>(Guid.NewGuid());
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

		public static explicit operator Guid(Id<TModel> id)
		{
			return id.Value;
		}

		public static explicit operator Id<TModel>(Guid id)
		{
			return new Id<TModel>(id);
		}

		public static explicit operator Id<TModel>(string id)
		{
			return new Id<TModel>(Guid.Parse(id));
		}

		public static bool operator ==(Id<TModel> a, Id<TModel> b)
		{
			return a.Value.Equals(b.Value);
		}

		public static bool operator !=(Id<TModel> a, Id<TModel> b)
		{
			return !a.Value.Equals(b.Value);
		}

		public Guid Value { get; set; }
	}
}
