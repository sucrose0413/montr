﻿using System;
using MediatR;
using Montr.Metadata.Models;
using Tendr.Models;

namespace Tendr.Queries
{
	public class GetEventList : IRequest<DataResult<Event>>
	{
		public Guid UserUid { get; set; }

		public EventSearchRequest Request { get; set; }
	}
}