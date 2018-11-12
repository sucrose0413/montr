using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Tendr.Services
{
	public static class QueryableExtensions
	{
		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderBy");
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "OrderByDescending");
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenBy");
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder<T>(source, property, "ThenByDescending");
		}

		private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
		{
			var type = typeof(T);
			var arg = Expression.Parameter(type, "x");
			Expression expr = arg;

			var props = property.Split('.');
			foreach (var prop in props)
			{
				// use reflection (not ComponentModel) to mirror LINQ
				var pi = type.GetProperty(prop,
					BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

				if (pi == null) throw new InvalidOperationException($"Property with name \"{prop}\" not found.");

				expr = Expression.Property(expr, pi);
				type = pi.PropertyType;
			}

			var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
			var lambda = Expression.Lambda(delegateType, expr, arg);

			var result = typeof(Queryable).GetMethods().Single(
					method => method.Name == methodName
							&& method.IsGenericMethodDefinition
							&& method.GetGenericArguments().Length == 2
							&& method.GetParameters().Length == 2)
				.MakeGenericMethod(typeof(T), type)
				.Invoke(null, new object[] { source, lambda });

			return (IOrderedQueryable<T>)result;
		}
	}
}