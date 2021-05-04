﻿using System;
using LinqToDB.Identity;
using LinqToDB.Mapping;

namespace Montr.Idx.Impl.Entities
{
	[Table(Schema = "montr", Name = "role_claim")]
	public class DbRoleClaim : IdentityRoleClaim<Guid>
	{
	}
}
