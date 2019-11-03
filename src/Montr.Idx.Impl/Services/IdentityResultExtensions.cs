﻿using System.Linq;
using Microsoft.AspNetCore.Identity;
using Montr.Core.Models;

namespace Montr.Idx.Impl.Services
{
	public static class IdentityResultExtensions
	{
		public static ApiResult ToApiResult(this IdentityResult identityResult)
		{
			return new ApiResult
			{
				Success = false,
				Errors = identityResult.Errors
					.Select(x => new ApiResultError
					{
						Key = x.Code,
						Messages = new[] { x.Description }
					}).ToArray()
			};
		}
	}
}