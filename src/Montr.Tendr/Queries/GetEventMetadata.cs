﻿using System;
using MediatR;
using Montr.Core.Models;

namespace Montr.Tendr.Queries
{
	public class GetEventMetadata : IRequest<DataView>
	{
		public Guid CompanyUid { get; set; }

		public Guid UserUid { get; set; }

		public string ViewId { get; set; }
	}
}
