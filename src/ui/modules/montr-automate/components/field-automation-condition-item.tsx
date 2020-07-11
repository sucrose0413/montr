import React from "react";
import { Form, Space, Select } from "antd";
import { FieldAutomationCondition } from "../models";
import { AutomationItemProps } from ".";

interface Props extends AutomationItemProps {
	condition: FieldAutomationCondition;
}

export class FieldAutomationConditionItem extends React.Component<Props> {

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
						<Select.Option value="StatusCode">Status</Select.Option>
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
						<Select.Option value="Draft">Draft</Select.Option>
						<Select.Option value="Published">Published</Select.Option>
						<Select.Option value="Completed">Completed</Select.Option>
						<Select.Option value="Closed">Closed</Select.Option>
					</Select>
				</Form.Item>

			</Space>
		);
	};
}