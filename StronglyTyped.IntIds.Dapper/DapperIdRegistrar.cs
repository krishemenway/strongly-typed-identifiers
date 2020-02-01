using Dapper;
using System;
using System.Linq;
using System.Reflection;

namespace StronglyTyped.IntIds.Dapper
{
	///<summary>Adds query support for Id&lt;T&gt; in Dapper</summary>
	public static class DapperIdRegistrar
	{
		/// <summary>Registers all Id&lt;T&gt; found using reflection for use in Dapper queries</summary>
		/// <param name="assemblies">Assemblies that can contain instances of Idlt;Tgt; to be used with Dapper</param>
		public static void RegisterAll(params Assembly[] assemblies)
		{
			var relevantTypes = FindRelevantIdTypes(assemblies);
			RegisterTypeHandlerForIds(relevantTypes);
		}

		/// <summary>Register specific Id&lt;T&gt; for use in Dapper queries. Ensure each Id&lt;T&gt; is registered before attempting to use them in Dapper queries.</summary>
		/// <typeparam name="TModel">Type the identifier is for (e.g. Person, Team)</typeparam>
		public static void RegisterTypeHandlerForId<TModel>()
		{
			SqlMapper.AddTypeHandler(typeof(Id<TModel>), new TypeHandlerForIdOf<TModel>());
		}

		/// <summary>Register specific Id&lt;T&gt;s for use in Dapper queries. Ensure each Id&lt;T&gt; is registered before attempting to use them in Dapper queries.</summary>
		/// <param name="idOfTTypes">One or many Id&lt;T&gt; types to be registered in Dapper</param>
		public static void RegisterTypeHandlerForIds(params Type[] idOfTTypes)
		{
			foreach (var propertyType in idOfTTypes)
			{
				var identifierKey = propertyType.GenericTypeArguments[0];
				var typeForTypeHandlerForId = typeof(TypeHandlerForIdOf<>).MakeGenericType(identifierKey);

				var typeHandlerForPropertyType = (SqlMapper.ITypeHandler)Activator.CreateInstance(typeForTypeHandlerForId);

				SqlMapper.AddTypeHandler(propertyType, typeHandlerForPropertyType);
			}
		}

		private static Type[] FindRelevantIdTypes(Assembly[] assemblies)
		{
			var allTypes = assemblies.SelectMany(assembly => assembly.DefinedTypes);

			var idPropertyTypes = allTypes.SelectMany(type => type.DeclaredProperties).Select(property => property.PropertyType);
			var idFieldTypes = allTypes.SelectMany(type => type.DeclaredFields).Select(field => field.FieldType);

			var idAssembly = Assembly.Load("StronglyTyped.IntIds");
			return idPropertyTypes.Concat(idFieldTypes)
				.Select(UnwrapNullableType)
				.Where(x => x.Assembly.Equals(idAssembly))
				.Distinct().ToArray();
		}

		private static Type UnwrapNullableType(Type type)
		{
			var underlyingType = Nullable.GetUnderlyingType(type);
			return underlyingType != null ? underlyingType : type;
		}
	}
}
