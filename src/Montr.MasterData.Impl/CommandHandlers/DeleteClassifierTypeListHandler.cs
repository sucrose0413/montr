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
using Montr.Metadata.Models;

namespace Montr.MasterData.Impl.CommandHandlers
{
	public class DeleteClassifierTypeListHandler : IRequestHandler<DeleteClassifierTypeList, ApiResult>
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IDbContextFactory _dbContextFactory;

		public DeleteClassifierTypeListHandler(IUnitOfWorkFactory unitOfWorkFactory, IDbContextFactory dbContextFactory)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_dbContextFactory = dbContextFactory;
		}

		public async Task<ApiResult> Handle(DeleteClassifierTypeList request, CancellationToken cancellationToken)
		{
			if (request.UserUid == Guid.Empty) throw new InvalidOperationException("User is required.");
			if (request.CompanyUid == Guid.Empty) throw new InvalidOperationException("Company is required.");

			// todo: check company belongs to user

			using (var scope = _unitOfWorkFactory.Create())
			{
				int affected;

				using (var db = _dbContextFactory.Create())
				{
					// delete all groups
					await (
						from @group in db.GetTable<DbClassifierGroup>()
						join tree in db.GetTable<DbClassifierTree>() on @group.TreeUid equals tree.Uid
						join type in db.GetTable<DbClassifierType>() on tree.TypeUid equals type.Uid
						where type.CompanyUid == request.CompanyUid && request.Uids.Contains(type.Uid)
						select @group
					).DeleteAsync(cancellationToken);

					// delete all trees
					await (
						from tree in db.GetTable<DbClassifierTree>()
						join type in db.GetTable<DbClassifierType>() on tree.TypeUid equals type.Uid
						where type.CompanyUid == request.CompanyUid && request.Uids.Contains(type.Uid)
						select tree
					).DeleteAsync(cancellationToken);

					// todo: remove items

					// delete type
					affected = await db.GetTable<DbClassifierType>()
						.Where(x => x.CompanyUid == request.CompanyUid && request.Uids.Contains(x.Uid))
						.DeleteAsync(cancellationToken);
				}

				// todo: (events)

				scope.Commit();

				return new ApiResult { Success = true, AffectedRows = affected };
			}
		}
	}
}