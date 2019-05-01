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

namespace Montr.MasterData.Impl.CommandHandlers
{
	public class DeleteClassifierGroupHandler : IRequestHandler<DeleteClassifierGroup, int>
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IClassifierTypeService _classifierTypeService;

		public DeleteClassifierGroupHandler(IUnitOfWorkFactory unitOfWorkFactory, IDbContextFactory dbContextFactory,
			IClassifierTypeService classifierTypeService)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_dbContextFactory = dbContextFactory;
			_classifierTypeService = classifierTypeService;
		}

		public async Task<int> Handle(DeleteClassifierGroup request, CancellationToken cancellationToken)
		{
			if (request.UserUid == Guid.Empty) throw new InvalidOperationException("User is required.");
			if (request.CompanyUid == Guid.Empty) throw new InvalidOperationException("Company is required.");

			// todo: check company belongs to user
			var type = await _classifierTypeService.GetClassifierType(request.CompanyUid, request.TypeCode, cancellationToken);

			using (var scope = _unitOfWorkFactory.Create())
			{
				int result = 0;

				using (var db = _dbContextFactory.Create())
				{
					if (type.HierarchyType == HierarchyType.Groups)
					{
						var closureTable = new ClosureTableHandler(db);

						var tree = await db.GetTable<DbClassifierTree>()
							.SingleAsync(x => x.TypeUid == type.Uid && x.Code == request.TreeCode, cancellationToken);

						var group = await db.GetTable<DbClassifierGroup>()
							.SingleAsync(x => x.TreeUid == tree.Uid && x.Uid == request.Uid, cancellationToken);

						await closureTable.Delete(group.Uid, group.ParentUid, cancellationToken);

						// reset parent of groups
						await db.GetTable<DbClassifierGroup>()
							.Where(x => x.ParentUid == group.Uid)
							.Set(x => x.ParentUid, group.ParentUid)
							.UpdateAsync(cancellationToken);

						// reset parent of items
						if (group.ParentUid != null)
						{
							await db.GetTable<DbClassifierLink>()
								.Where(x => x.GroupUid == group.Uid)
								.Set(x => x.GroupUid, group.ParentUid)
								.UpdateAsync(cancellationToken);
						}
						else
						{
							await db.GetTable<DbClassifierLink>()
								.Where(x => x.GroupUid == group.Uid)
								.DeleteAsync(cancellationToken);
						}

						result = await db.GetTable<DbClassifierGroup>()
							.Where(x => x.Uid == group.Uid)
							.DeleteAsync(cancellationToken);
					}
				}

				// todo: (события)

				scope.Commit();

				return result;
			}
		}
	}
}
