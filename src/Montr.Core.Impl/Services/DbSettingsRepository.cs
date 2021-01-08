﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Montr.Core.Impl.Entities;
using Montr.Core.Services;
using Montr.Data.Linq2Db;

namespace Montr.Core.Impl.Services
{
	public class DbSettingsRepository : IOptionsRepository
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IDbContextFactory _dbContextFactory;

		public DbSettingsRepository(IUnitOfWorkFactory unitOfWorkFactory, IDbContextFactory dbContextFactory)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_dbContextFactory = dbContextFactory;
		}

		public IUpdatableOptions<TOptions> GetOptions<TOptions>()
		{
			return new UpdatableOptions<TOptions>(this);
		}

		public async Task<int> Update<TOptions>(IEnumerable<(string, object)> values, CancellationToken cancellationToken)
		{
			var affected = 0;

			using (var scope = _unitOfWorkFactory.Create())
			{
				using (var db = _dbContextFactory.Create())
				{
					foreach (var (key, value) in values)
					{
						var stringValue = value != null ? Convert.ToString(value) : null;

						var updated = await db.GetTable<DbSettings>()
							.Where(x => x.Id == key)
							.Set(x => x.Value, stringValue)
							.UpdateAsync(cancellationToken);

						affected += updated;

						if (updated == 0)
						{
							var inserted = await db.GetTable<DbSettings>()
								.Value(x => x.Id, key)
								.Value(x => x.Value, stringValue)
								.InsertAsync(cancellationToken);

							affected += inserted;
						}
					}
				}

				scope.Commit();
			}

			// todo: notify settings changed to reload from db
			return affected;
		}

		private class UpdatableOptions<TOptions> : IUpdatableOptions<TOptions>
		{
			private readonly IOptionsRepository _repository;

			private readonly IList<(string, object)> _values = new List<(string, object)>();

			public UpdatableOptions(IOptionsRepository repository)
			{
				_repository = repository;
			}

			public IUpdatableOptions<TOptions> Set<TValue>(Expression<Func<TOptions, TValue>> keyExpr, TValue value)
			{
				var key = typeof(TOptions).FullName + "." + ExpressionHelper.GetMemberName(keyExpr);

				_values.Add((key, value));

				return this;
			}

			public async Task<int> Update(CancellationToken cancellationToken)
			{
				return await _repository.Update<TOptions>(_values, cancellationToken);
			}
		}
	}
}