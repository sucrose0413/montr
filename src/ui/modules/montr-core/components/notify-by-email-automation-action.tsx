import React from "react";
import { Space, Input, Form } from "antd";
import { INotifyByEmailAutomationAction } from "../models";
import { IAutomationActionProps } from "./automation-field-factory";
import TextArea from "antd/lib/input/TextArea";

interface IProps extends IAutomationActionProps {
	action: INotifyByEmailAutomationAction;
}

export class NotifyByEmailAutomationAction extends React.Component<IProps> {

	render = () => {
		const { typeSelector, item } = this.props;

		return (
			<Space style={{ display: "flex" }} align="start">

				{typeSelector}

				<Form.Item
					{...item}
					name={[item.name, "recipient"]}
					fieldKey={[item.fieldKey, "recipient"]}
					rules={[{ required: true }]}>
					<Input placeholder="Recipient" />
				</Form.Item>

				<Form.Item
					{...item}
					label="Subject"
					name={[item.name, "subject"]}
					fieldKey={[item.fieldKey, "subject"]}
					rules={[{ required: true }]}>
					<Input placeholder="Subject" />
				</Form.Item>

				<Form.Item
					{...item}
					label="Body"
					name={[item.name, "body"]}
					fieldKey={[item.fieldKey, "body"]}
					rules={[{ required: true }]}>
					<TextArea placeholder="Body" />
				</Form.Item>

			</Space>
		);
	};
}