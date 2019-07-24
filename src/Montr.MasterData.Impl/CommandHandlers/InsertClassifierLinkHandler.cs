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
using Montr.MasterData.Models;
using Montr.MasterData.Services;
using Montr.Metadata.Models;

namespace Montr.MasterData.Impl.CommandHandlers
{
	public class InsertClassifierLinkHandler : IRequestHandler<InsertClassifierLink, ApiResult>
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IClassifierTypeService _classifierTypeService;

		public InsertClassifierLinkHandler(IUnitOfWorkFactory unitOfWorkFactory, IDbContextFactory dbContextFactory,
			IClassifierTypeService classifierTypeService)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_dbContextFactory = dbContextFactory;
			_classifierTypeService = classifierTypeService;
		}

		public async Task<ApiResult> Handle(InsertClassifierLink request, CancellationToken cancellationToken)
		{
			if (request.UserUid == Guid.Empty) throw new InvalidOperationException("User is required.");
			if (request.CompanyUid == Guid.Empty) throw new InvalidOperationException("Company is required.");

			// todo: check company belongs to user
			var type = await _classifierTypeService.GetClassifierType(request.CompanyUid, request.TypeCode, cancellationToken);

			using (var scope = _unitOfWorkFactory.Create())
			{
				using (var db = _dbContextFactory.Create())
				{
					// todo: validate group and item belongs to the same type

					if (type.HierarchyType != HierarchyType.Groups)
					{
						throw new InvalidOperationException("Invalid classifier hierarchy type for groups operations.");
					}

					// delete other links in same hierarchy
					var deleted = await (
						from link in db.GetTable<DbClassifierLink>().Where(x => x.ItemUid == request.ItemUid)
						join allGroups in db.GetTable<DbClassifierGroup>() on link.GroupUid equals allGroups.Uid
						join newGroup in db.GetTable<DbClassifierGroup>().Where(x => x.Uid == request.GroupUid)
							on allGroups.TreeUid equals newGroup.TreeUid
						select link
					).DeleteAsync(cancellationToken);

					var inserted = await db.GetTable<DbClassifierLink>()
						.Value(x => x.GroupUid, request.GroupUid)
						.Value(x => x.ItemUid, request.ItemUid)
						.InsertAsync(cancellationToken);
				}

				// todo: events

				scope.Commit();

				return new ApiResult { Success = true };
			}
		}
	}
}