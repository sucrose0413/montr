import * as React from "react";
import { Input, InputNumber, Select, Checkbox, DatePicker } from "antd";
import { IDataField, IIndexer, ISelectField, ITextAreaField, INumberField, IDateField, IBooleanField, ITextField, IDesignSelectOptionsField, IPasswordField } from "../models";
import { Icon, DesignSelectOptions, EmptyFieldView } from ".";
import moment from "moment";
import { DataHelper } from "@montr-core/services";

export abstract class DataFieldFactory<TField extends IDataField> {
	private static Map: { [key: string]: DataFieldFactory<IDataField>; } = {};

	static register(key: string, factory: DataFieldFactory<IDataField>) {
		DataFieldFactory.Map[key] = factory;
	}

	static get(key: string): DataFieldFactory<IDataField> {
		return DataFieldFactory.Map[key];
	}

	valuePropName: string = "value";

	shouldFormatValue: boolean = false;

	formatValue(field: TField, data: IIndexer, value: any): any {
		return value;
	}

	createViewNode(field: TField, data: IIndexer): React.ReactElement {
		const value = DataHelper.indexer(data, field.key, undefined);

		return (value != undefined) ? value : <EmptyFieldView />;
	}

	abstract createEditNode(field: TField, data: IIndexer): React.ReactElement;
}

class BooleanFieldFactory extends DataFieldFactory<IBooleanField> {
	constructor() {
		super();
		this.valuePropName = "checked";
	}

	createEditNode(field: IBooleanField, data: IIndexer): React.ReactElement {
		return <Checkbox>{field.name}</Checkbox>;
	}
}

class TextFieldFactory extends DataFieldFactory<ITextField> {
	createEditNode(field: ITextField, data: IIndexer): React.ReactElement {
		return <Input
			allowClear
			disabled={field.readonly}
			placeholder={field.placeholder}
			prefix={field.icon && Icon.get(field.icon)}
		/>;
	}
}

class NumberFieldFactory extends DataFieldFactory<INumberField> {
	createEditNode(field: INumberField, data: IIndexer): React.ReactElement {
		const props = field?.props;

		return <InputNumber
			min={props?.min}
			max={props?.max}
			disabled={field.readonly}
			placeholder={field.placeholder}
		/>;
	}
}

class TextAreaFieldFactory extends DataFieldFactory<ITextAreaField> {
	createEditNode(field: ITextAreaField, data: IIndexer): React.ReactElement {
		const props = field?.props;

		return <Input.TextArea
			allowClear
			placeholder={field.placeholder}
			autoSize={{ minRows: props?.rows || 4, maxRows: 12 }}
		/>;
	}
}

class SelectFieldFactory extends DataFieldFactory<ISelectField> {
	createEditNode(field: ISelectField, data: IIndexer): React.ReactElement {
		const props = field?.props;

		return <Select
			// allowClear
			showSearch
			placeholder={field?.placeholder}
			style={{ minWidth: 200, width: "auto" }}>
			{props?.options.map(x => {
				return <Select.Option key={x.value} value={x.value}>{x.name || x.value}</Select.Option>;
			})}
		</Select>;
	}

	createViewNode(field: ISelectField, data: IIndexer): React.ReactElement {
		const value = DataHelper.indexer(data, field.key, undefined);

		const option = field.props.options.find(x => x.value == value);

		if (option) {
			return <>{option.name}</>;
		}

		return (value) ? value : <EmptyFieldView />;
	}
}

class DesignSelectOptionsFieldFactory extends DataFieldFactory<IDesignSelectOptionsField> {
	createEditNode(field: IDesignSelectOptionsField, data: IIndexer): React.ReactElement {
		return <DesignSelectOptions />;
	}
}

class PasswordFieldFactory extends DataFieldFactory<IPasswordField> {
	createEditNode(field: IDataField, data: IIndexer): React.ReactElement {
		return <Input.Password
			allowClear
			placeholder={field.placeholder}
			prefix={field.icon && Icon.get(field.icon)}
		/>;
	}
}

class DateFieldFactory extends DataFieldFactory<IDateField> {
	constructor() {
		super();
		this.shouldFormatValue = true;
	}

	formatValue(field: IDateField, data: IIndexer, value: any): any {
		return value ? moment.parseZone(value) : null;
	}

	createEditNode(field: IDateField, data: IIndexer): React.ReactElement {
		const props = field?.props;

		return <DatePicker
			allowClear
			showTime={props?.includeTime}
			disabled={field.readonly}
			placeholder={field.placeholder}
		/>;
	}

	createViewNode(field: IDateField, data: IIndexer): React.ReactElement {
		const value = DataHelper.indexer(data, field.key, undefined);

		return (value != undefined)
			? value.format(field.props.includeTime ? "LLL" : "L")
			: <EmptyFieldView />;
	}
}

/* class TimeFieldFactory extends DataFieldFactory<ITimeField> {
	constructor() {
		super();
		this.shouldFormatValue = true;
	}

	formatValue(field: ITimeField, data: IIndexer, value: any): any {
		return value ? moment.parseZone(value, "HH:mm:ss") : null;
	}

	createEditNode(field: ITimeField, data: IIndexer): React.ReactElement {
		return <TimePicker
			allowClear
			disabled={field.readonly}
			placeholder={field.placeholder}
		/>;
	}

	createViewNode(field: ITimeField, data: IIndexer): React.ReactElement {
		const value = DataHelper.indexer(data, field.key, undefined);

		return (value != undefined)
			? value.format("LTS")
			: <EmptyFieldView />;
	}
} */

DataFieldFactory.register("boolean", new BooleanFieldFactory());
DataFieldFactory.register("number", new NumberFieldFactory());
DataFieldFactory.register("text", new TextFieldFactory());
DataFieldFactory.register("textarea", new TextAreaFieldFactory());
DataFieldFactory.register("select", new SelectFieldFactory());
DataFieldFactory.register("select-options", new DesignSelectOptionsFieldFactory());
DataFieldFactory.register("password", new PasswordFieldFactory());
DataFieldFactory.register("date", new DateFieldFactory());
// DataFieldFactory.register("time", new TimeFieldFactory());
