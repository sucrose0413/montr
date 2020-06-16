import React from "react";
import { Divider, Form, Select } from "antd";
import { ButtonAdd, Toolbar, FormListItemToolbar, IDataFormOptions } from "@montr-core/components";
import { IAutomationActionListField } from "../models";
import { AutomationAction } from ".";

interface IProps {
	field: IAutomationActionListField;
	options: IDataFormOptions;
}

export class AutomationActionList extends React.Component<IProps> {

	render = () => {
		const { field, options } = this.props;

		return (
			<Form.List name={field.key}>
				{(items, { add, remove, move }) => {

					return (<>

						{field.name && <Divider orientation="left">{field.name}</Divider>}

						{items.map((item, index) => {

							const typeSelector = (
								<Form.Item
									{...item}
									name={[item.name, "type"]}
									fieldKey={[item.fieldKey, "type"]}
									rules={[{ required: true }]}>
									<Select placeholder="Select action" style={{ minWidth: 150 }}>
										<Select.Option value="set-field">Set Field</Select.Option>
										<Select.Option value="notify-by-email">Notify By Email</Select.Option>
									</Select>
								</Form.Item>
							);

							return (
								<div key={item.key}>

									<FormListItemToolbar
										item={item}
										itemIndex={index}
										itemsCount={items.length}
										ops={{ remove, move }} />

									<Form.Item
										{...item}
										name={[item.name]}
										fieldKey={[item.fieldKey]}
										rules={[{ required: true }]}
										noStyle>
										<AutomationAction item={item} typeSelector={typeSelector} options={options} />
									</Form.Item>
								</div>
							);
						})}

						<Toolbar>
							<ButtonAdd type="dashed" onClick={() => add()}>Add action</ButtonAdd>
						</Toolbar>
					</>);
				}}
			</Form.List>

		);
	};
}
