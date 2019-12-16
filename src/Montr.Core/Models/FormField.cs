﻿namespace Montr.Core.Models
{
	// todo: rename to DataField (?)
	public abstract class FormField
	{
		public abstract string Type { get; }

		public string Key { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Placeholder { get; set; }

		public string Icon { get; set; }

		public bool Multiple { get; set; }

		public bool Readonly { get; set; }

		public bool Required { get; set; }
	}

	public class BooleanField : FormField
	{
		public override string Type => "boolean";
	}

	public class StringField : FormField
	{
		public override string Type => "string";

		public bool Autosize { get; set; }
	}

	// todo join with StringField?
	public class TextAreaField : StringField
	{
		public override string Type => "textarea";

		public byte? Rows { get; set; }
	}

	public class PasswordField : FormField
	{
		public override string Type => "password";
	}

	public class NumberField : FormField
	{
		public override string Type => "number";

		public long? Min { get; set; }

		public long? Max { get; set; }
	}

	public class DecimalField : FormField
	{
		public override string Type => "decimal";

		public decimal? Min { get; set; }

		public decimal? Max { get; set; }

		public byte? Precision { get; set; }
	}

	public class DateField : FormField
	{
		public override string Type => "date";
	}

	public class TimeField : FormField
	{
		public override string Type => "time";
	}

	public class DateTimeField : FormField
	{
		public override string Type => "datetime";
	}

	public class SelectField : FormField
	{
		public override string Type => "select";

		public SelectFieldOption[] Options { get; set; }
	}

	public class SelectFieldOption
	{
		public string Value { get; set; }

		public string Name { get; set; }
	}

	public class ClassifierGroupField : FormField
	{
		public override string Type => "classifier-group";

		public string TypeCode { get; set; }

		public string TreeCode { get; set; }
	}

	public class ClassifierField : FormField
	{
		public override string Type => "classifier";

		public string TypeCode { get; set; }
	}

	public class FileField : FormField
	{
		public override string Type => "file";
	}
}
