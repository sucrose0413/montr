import React from "react";
import { Form, Space, Select } from "antd";
import { IFieldAutomationCondition } from "../models";
import { IAutomationItemProps } from ".";

interface IProps extends IAutomationItemProps {
	condition: IFieldAutomationCondition;
}

export class FieldAutomationCondition extends React.Component<IProps> {

	render = () => {
		const { typeSelector, item } = this.props;

		return (
			<Space align="start">

				{typeSelector}

				<Form.Item
					{...item}
					name={[item.name, "props", "field"]}
					fieldKey={[item.fieldKey, "field"]}
					rules={[{ required: true }]}>
					<Select placeholder="Select field" style={{ minWidth: 100 }}>
						<Select.Option value="status">Status</Select.Option>
					</Select>
				</Form.Item>

				<Form.Item
					{...item}
					name={[item.name, "props", "operator"]}
					fieldKey={[item.fieldKey, "operator"]}
					rules={[{ required: true }]}>
					<Select style={{ minWidth: 50 }}>
						<Select.Option value="Equal">=</Select.Option>
						<Select.Option value="NotEqual">&lt;&gt;</Select.Option>
						<Select.Option value="LessThan">&lt;</Select.Option>
						<Select.Option value="LessThanEqual">&lt;=</Select.Option>
						<Select.Option value="GreaterThan">&gt;</Select.Option>
						<Select.Option value="GreaterThanEqual">&gt;=</Select.Option>
					</Select>
				</Form.Item>

				<Form.Item
					{...item}
					name={[item.name, "props", "value"]}
					fieldKey={[item.fieldKey, "value"]}
					rules={[{ required: true }]}>
					<Select placeholder="Select value" style={{ minWidth: 100 }}>
						<Select.Option value="draft">Draft</Select.Option>
						<Select.Option value="published">Published</Select.Option>
						<Select.Option value="completed">Completed</Select.Option>
						<Select.Option value="closed">Closed</Select.Option>
					</Select>
				</Form.Item>

			</Space>
		);
	};
}
