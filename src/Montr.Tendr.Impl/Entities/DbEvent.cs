﻿using LinqToDB;
using LinqToDB.Mapping;

namespace Montr.Tendr.Impl.Entities
{
	[Table(Name = "event")]
	public class DbEvent
	{
		[Column(Name = "uid"), DataType(DataType.Guid), PrimaryKey, Identity]
		public System.Guid Uid { get; set; }

		[Column(Name = "id"), DataType(DataType.Int64)]
		public long Id { get; set; }

		[Column(Name = "company_uid"), DataType(DataType.Guid)]
		public System.Guid CompanyUid { get; set; }

		[Column(Name = "config_code"), DataType(DataType.VarChar), NotNull]
		public string ConfigCode { get; set; }

		[Column(Name = "status_code"), DataType(DataType.VarChar), NotNull]
		public string StatusCode { get; set; }

		[Column(Name = "name"), DataType(DataType.VarChar), Nullable]
		public string Name { get; set; }

		[Column(Name = "description"), DataType(DataType.VarChar), Nullable]
		public string Description { get; set; }
	}
}