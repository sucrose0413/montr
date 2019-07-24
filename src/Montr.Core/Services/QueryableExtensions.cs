﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Montr.Core.Models;

namespace Montr.Core.Services
{
	public static class QueryableExtensions
	{
		// todo: pass viewId instead of defaultSortColumn
		public static IQueryable<T> Apply<T>(this IQueryable<T> source, Paging paging,
			Expression<Func<T, object>> defaultSortColumn, SortOrder defaultSortOrder = SortOrder.Ascending)
		{
			if (paging == null)
			{
				paging = new Paging();
			}

			if (paging.PageNo <= 0)
			{
				paging.PageNo = 1;
			}

			if (paging.PageSize <= 0 || paging.PageSize > Paging.MaxPageSize)
			{
				paging.PageSize = Paging.DefaultPageSize;
			}

			if (paging.SortColumn == null)
			{
				paging.SortColumn = GetMemberName(defaultSortColumn);
			}

			if (paging.SortOrder == null)
			{
				paging.SortOrder = defaultSortOrder;
			}

			var ordered = paging.SortOrder == SortOrder.Ascending
				? source.OrderBy(paging.SortColumn)
				: source.OrderByDescending(paging.SortColumn);

			var paged = ordered
				.Skip((paging.PageNo - 1) * paging.PageSize)
				.Take(paging.PageSize);

			return paged;
		}

		public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder(source, property, "OrderBy");
		}

		public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
		{
			return ApplyOrder(source, property, "OrderByDescending");
		}

		public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder(source, property, "ThenBy");
		}

		public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
		{
			return ApplyOrder(source, property, "ThenByDescending");
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

		private static string GetMemberName(Expression expression)
		{
			var lambda = expression as LambdaExpression;

			if (lambda == null) throw new ArgumentNullException(nameof(expression));

			MemberExpression memberExpression = null;

			if (lambda.Body.NodeType == ExpressionType.Convert)
			{
				memberExpression = ((UnaryExpression)lambda.Body).Operand as MemberExpression;
			}
			else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
			{
				memberExpression = lambda.Body as MemberExpression;
			}
			
			if (memberExpression == null)
			{
				throw new ArgumentException(nameof(expression));
			}
			
			return memberExpression.Member.Name;
		}
	}
}