﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using MediatR;
using Montr.Core.Services;
using Montr.Data.Linq2Db;
using Montr.MasterData.Commands;
using Montr.MasterData.Impl.Entities;
using Montr.MasterData.Impl.Services;
using Montr.MasterData.Models;
using Montr.MasterData.Services;
using Montr.Metadata.Models;

namespace Montr.MasterData.Impl.CommandHandlers
{
	public class UpdateClassifierHandler : IRequestHandler<UpdateClassifier, ApiResult>
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IClassifierTypeService _classifierTypeService;
		private readonly IClassifierTreeService _classifierTreeService;

		public UpdateClassifierHandler(IUnitOfWorkFactory unitOfWorkFactory, IDbContextFactory dbContextFactory,
			IClassifierTypeService classifierTypeService, IClassifierTreeService classifierTreeService)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_dbContextFactory = dbContextFactory;
			_classifierTypeService = classifierTypeService;
			_classifierTreeService = classifierTreeService;
		}

		public async Task<ApiResult> Handle(UpdateClassifier request, CancellationToken cancellationToken)
		{
			if (request.UserUid == Guid.Empty) throw new InvalidOperationException("User is required.");
			if (request.CompanyUid == Guid.Empty) throw new InvalidOperationException("Company is required.");

			var item = request.Item ?? throw new ArgumentNullException(nameof(request.Item));

			// todo: check company belongs to user
			var type = await _classifierTypeService.GetClassifierType(request.CompanyUid, request.TypeCode, cancellationToken);

			var tree = type.HierarchyType == HierarchyType.Groups
				? await _classifierTreeService.GetClassifierTree(request.CompanyUid, request.TypeCode, ClassifierTree.DefaultCode, cancellationToken)
				: null;

			using (var scope = _unitOfWorkFactory.Create())
			{
				int affected;

				using (var db = _dbContextFactory.Create())
				{
					var validator = new ClassifierValidator(db, type);

					if (await validator.ValidateUpdate(item, cancellationToken) == false)
					{
						return new ApiResult { Success = false, Errors = validator.Errors };
					}

					affected = await db.GetTable<DbClassifier>()
						.Where(x => x.Uid == item.Uid)
						.Set(x => x.Code, item.Code)
						.Set(x => x.Name, item.Name)
						.Set(x => x.ParentUid, type.HierarchyType == HierarchyType.Items ? item.ParentUid : null)
						.UpdateAsync(cancellationToken);

					if (type.HierarchyType == HierarchyType.Groups)
					{
						// todo: combine with InsertClassifierLinkHandler in one service

						// delete other links in same tree
						var deleted = await (
							from link in db.GetTable<DbClassifierLink>().Where(x => x.ItemUid == item.Uid)
							join groups in db.GetTable<DbClassifierGroup>() on link.GroupUid equals groups.Uid
							where groups.TreeUid == tree.Uid
							select link
						).DeleteAsync(cancellationToken);

                        // todo: check parent belongs to default tree
						if (item.ParentUid != null)
						{
							var inserted = await db.GetTable<DbClassifierLink>()
								.Value(x => x.GroupUid, item.ParentUid)
								.Value(x => x.ItemUid, item.Uid)
								.InsertAsync(cancellationToken);
						}
					}
					else if (type.HierarchyType == HierarchyType.Items)
					{
						var closureTable = new ClosureTableHandler(db, type);

						// ReSharper disable once PossibleInvalidOperationException
						if (await closureTable.Update(item.Uid.Value, item.ParentUid, cancellationToken) == false)
						{
							return new ApiResult { Success = false, Errors = closureTable.Errors };
						}
					}
				}

				// todo: (события)

				scope.Commit();

				return new ApiResult { Success = true, AffectedRows = affected };
			}
		}
	}
}