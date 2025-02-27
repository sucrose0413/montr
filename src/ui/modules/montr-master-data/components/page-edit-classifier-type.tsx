import { DataTabs, Page, PageHeader } from "@montr-core/components";
import { DataView } from "@montr-core/models";
import { Spin } from "antd";
import * as React from "react";
import { RouteComponentProps } from "react-router";
import { ClassifierBreadcrumb } from ".";
import { ClassifierType } from "../models";
import { RouteBuilder, Views } from "../module";
import { ClassifierTypeService } from "../services";

interface RouteProps {
	uid?: string;
	tabKey?: string;
}

interface Props extends RouteComponentProps<RouteProps> {
}

interface State {
	loading: boolean;
	dataView?: DataView<ClassifierType>;
	types?: ClassifierType[];
	data?: ClassifierType;
}

export default class EditClassifierType extends React.Component<Props, State> {

	private readonly classifierTypeService = new ClassifierTypeService();

	constructor(props: Props) {
		super(props);

		this.state = {
			loading: true
		};
	}

	componentDidMount = async (): Promise<void> => {
		await this.fetchData();
	};

	componentDidUpdate = async (prevProps: Props): Promise<void> => {
		if (this.props.match.params.uid !== prevProps.match.params.uid) {
			await this.fetchData();
		}
	};

	componentWillUnmount = async (): Promise<void> => {
		await this.classifierTypeService.abort();
	};

	fetchData = async (): Promise<void> => {
		const { uid } = this.props.match.params;

		const types = await this.classifierTypeService.list({ skipPaging: true });

		const data: ClassifierType = (uid)
			? await this.classifierTypeService.get({ uid })
			: await this.classifierTypeService.create();

		const dataView = await this.classifierTypeService.metadata(data.code, Views.classifierTypeTabs);

		this.setState({ loading: false, dataView, data, types: types.rows });
	};

	handleDataChange = (data: ClassifierType): void => {
		this.setState({ data });
	};

	handleTabChange = (tabKey: string): void => {
		const { uid } = this.props.match.params;

		const path = RouteBuilder.editClassifierType(uid, tabKey);

		this.props.history.replace(path);
	};

	render = (): React.ReactNode => {
		const { uid, tabKey } = this.props.match.params,
			{ loading, dataView, data, types } = this.state;

		let title;
		// todo: remove this sh*t
		if (loading) {
			title = <>
				<ClassifierBreadcrumb /* types={types} */ />
				<PageHeader>&#xA0;</PageHeader>
			</>;
		}
		else {
			title = <>
				{(uid)
					? <ClassifierBreadcrumb types={types} type={data} item={{ name: "Настройка" }} />
					: <ClassifierBreadcrumb item={{ name: "Добавление" }} />
				}
				<PageHeader>{data.name}</PageHeader>
			</>;
		}

		return (
			<Page title={title}>
				<Spin spinning={loading}>

					<DataTabs
						tabKey={tabKey}
						panes={dataView?.panes}
						onTabChange={this.handleTabChange}
						disabled={(_, index) => index > 0 && !data?.uid}
						tabProps={{
							data,
							onDataChange: this.handleDataChange,
							entityTypeCode: `ClassifierType`,
							entityUid: data?.uid
						}}
					/>

				</Spin>
			</Page>
		);
	};
}
