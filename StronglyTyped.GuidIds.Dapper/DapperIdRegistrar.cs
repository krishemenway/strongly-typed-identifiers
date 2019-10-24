using Dapper;
using System;
using System.Linq;
using System.Reflection;

namespace StronglyTyped.GuidIds.Dapper
{
	///<summary>Adds query support for Id&lt;T&gt; in Dapper</summary>
	public static class DapperIdRegistrar
	{
		/// <summary>Registers all Id&lt;T&gt; found using reflection for use in Dapper queries</summary>
		/// <param name="assemblies">Assemblies that can contain instances of Idlt;Tgt; to be used with Dapper</param>
		public static void RegisterAll(params Assembly[] assemblies)
		{
			var idAssembly = Assembly.Load("StronglyTyped.GuidIds");
			var idPropertyTypes = assemblies
				.SelectMany(assembly => assembly.DefinedTypes)
				.SelectMany(type => type.DeclaredProperties)
				.Select(property => property.PropertyType)
				.Where(x => x.Assembly.Equals(idAssembly))
				.Distinct();

			RegisterTypeHandlerForIds(idPropertyTypes.ToArray());
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

				var typeHandlerForPropertyType = (SqlMapper.ITypeHandler) Activator.CreateInstance(typeForTypeHandlerForId);

				SqlMapper.AddTypeHandler(propertyType, typeHandlerForPropertyType);
			}
		}
	}
}
