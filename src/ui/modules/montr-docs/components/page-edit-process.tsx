import React from "react";
import { RouteComponentProps } from "react-router";
import { Page, PaneSearchMetadata } from "@montr-core/components";
import { Spin, Tabs } from "antd";
import { RouteBuilder } from "@montr-docs/module";

interface IRouteProps {
	uid?: string;
	tabKey?: string;
}

interface IProps extends RouteComponentProps<IRouteProps> {
}

interface IState {
	loading: boolean;
}

export default class EditClassifierType extends React.Component<IProps, IState> {

	constructor(props: IProps) {
		super(props);

		this.state = {
			loading: false
		};
	}

	handleTabChange = (tabKey: string) => {
		const { uid } = this.props.match.params;

		const path = RouteBuilder.editProcess(uid, tabKey);

		this.props.history.replace(path);
	};

	render = () => {
		const { uid, tabKey } = this.props.match.params,
			{ loading } = this.state;

		const otherTabsDisabled = !uid;

		return (
			<Page title={`${uid}`}>
				<Spin spinning={loading}>
					<Tabs size="small" defaultActiveKey={tabKey} onChange={this.handleTabChange}>
						<Tabs.TabPane key="fields" tab="Поля">
							<PaneSearchMetadata entityTypeCode={`Process`} entityUid={uid} />
						</Tabs.TabPane>
					</Tabs>
				</Spin>
			</Page>
		);
	};
}
