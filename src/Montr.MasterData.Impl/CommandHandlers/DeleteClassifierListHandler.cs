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
using Montr.MasterData.Services;
using Montr.Metadata.Models;

namespace Montr.MasterData.Impl.CommandHandlers
{
	public class DeleteClassifierListHandler : IRequestHandler<DeleteClassifierList, ApiResult>
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IClassifierTypeService _classifierTypeService;

		public DeleteClassifierListHandler(IUnitOfWorkFactory unitOfWorkFactory, IDbContextFactory dbContextFactory,
			IClassifierTypeService classifierTypeService)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_dbContextFactory = dbContextFactory;
			_classifierTypeService = classifierTypeService;
		}

		public async Task<ApiResult> Handle(DeleteClassifierList request, CancellationToken cancellationToken)
		{
			if (request.UserUid == Guid.Empty) throw new InvalidOperationException("User is required.");
			if (request.CompanyUid == Guid.Empty) throw new InvalidOperationException("Company is required.");

			// todo: check company belongs to user
			var type = await _classifierTypeService.GetClassifierType(request.CompanyUid, request.TypeCode, cancellationToken);

			using (var scope = _unitOfWorkFactory.Create())
			{
				int affected;

				using (var db = _dbContextFactory.Create())
				{
					affected = await db.GetTable<DbClassifier>()
						.Where(x => x.TypeUid == type.Uid && request.Uids.Contains(x.Uid))
						.DeleteAsync(cancellationToken);
				}

				// todo: (события)

				scope.Commit();

				return new ApiResult { Success = true, AffectedRows = affected };
			}
		}
	}
}