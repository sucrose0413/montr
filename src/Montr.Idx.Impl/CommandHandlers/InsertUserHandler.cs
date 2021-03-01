﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Montr.Core.Models;
using Montr.Core.Services;
using Montr.Idx.Commands;
using Montr.Idx.Services;

namespace Montr.Idx.Impl.CommandHandlers
{
	public class InsertUserHandler : IRequestHandler<InsertUser, ApiResult>
	{
		private readonly IUnitOfWorkFactory _unitOfWorkFactory;
		private readonly IUserManager _userManager;

		public InsertUserHandler(IUnitOfWorkFactory unitOfWorkFactory, IUserManager userManager)
		{
			_unitOfWorkFactory = unitOfWorkFactory;
			_userManager = userManager;
		}

		public async Task<ApiResult> Handle(InsertUser request, CancellationToken cancellationToken)
		{
			var user = request?.Item ?? throw new ArgumentNullException(nameof(request));

			using (var scope = _unitOfWorkFactory.Create())
			{
				var result = await _userManager.Create(user, cancellationToken);

				if (result.Success) scope.Commit();

				return result;
			}
		}
	}
}