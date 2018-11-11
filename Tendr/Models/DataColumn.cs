﻿namespace Tendr.Models
{
    public class DataColumn
    {
		public string Key { get; set; }

		public string Path { get; set; }

		public string Name { get; set; }

		public string UrlProperty { get; set; }

		public DataColumnAlign Align { get; set; }

		public DataColumnFixed? Fixed { get; set; }

		public bool Sortable { get; set; }

		public short? Width { get; set; }
	}

	public enum DataColumnAlign : byte
	{
		Left,
		Right,
		Center
	}

	public enum DataColumnFixed : byte
	{
		Left,
		Right
	}
}
