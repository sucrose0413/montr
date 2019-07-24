﻿using System;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MediatR;
using Montr.Core.Services;
using Montr.Data.Linq2Db;
using Montr.MasterData.Commands;
using Montr.MasterData.Impl.Entities;
using Montr.MasterData.Models;
using Montr.Metadata.Models;

namespace Montr.MasterData.Impl.CommandHandlers
{
	public class InsertClassifierTypeHandler : IRequestHandler<InsertClassifierType, ApiResult>
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IDbContextFactory _dbContextFactory;

		public InsertClassifierTypeHandler(IUnitOfWorkFactory unitOfWorkFactory, IDbContextFactory dbContextFactory)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_dbContextFactory = dbContextFactory;
		}

		public async Task<ApiResult> Handle(InsertClassifierType request, CancellationToken cancellationToken)
		{
			if (request.UserUid == Guid.Empty) throw new InvalidOperationException("User is required.");
			if (request.CompanyUid == Guid.Empty) throw new InvalidOperationException("Company is required.");

			var item = request.Item ?? throw new ArgumentNullException(nameof(request.Item));

			using (var scope = _unitOfWorkFactory.Create())
			{
				var itemUid = Guid.NewGuid();

				// todo: validation and limits
				// todo: reserved codes (add, new etc. can conflict with routing)

				using (var db = _dbContextFactory.Create())
				{
					var validator = new ClassifierTypeValidator(db);

					if (await validator.ValidateInsert(item, cancellationToken) == false)
					{
						return new ApiResult { Success = false, Errors = validator.Errors };
					}

					// todo: change date

					await db.GetTable<DbClassifierType>()
						.Value(x => x.Uid, itemUid)
						.Value(x => x.CompanyUid, request.CompanyUid)
						.Value(x => x.Code, item.Code)
						.Value(x => x.Name, item.Name)
						.Value(x => x.Description, item.Description)
						.Value(x => x.HierarchyType, item.HierarchyType.ToString())
						.InsertAsync(cancellationToken);

					if (item.HierarchyType == HierarchyType.Groups)
					{
						var treeUid = Guid.NewGuid();

						// todo: move to some MasterDataService
						await db.GetTable<DbClassifierTree>()
							.Value(x => x.Uid, treeUid)
							.Value(x => x.TypeUid, itemUid)
							.Value(x => x.Code, ClassifierTree.DefaultCode)
							.Value(x => x.Name, item.Name)
							.InsertAsync(cancellationToken);
					}
				}

				// todo: (events)

				scope.Commit();

				return new ApiResult { Success = true, Uid = itemUid };
			}
		}
	}
}