import * as React from "react";
import { Redirect } from "react-router";
import { Spin } from "antd";
import { CompanyContextProps, withCompanyContext } from "@kompany/components";
import { DataForm } from "@montr-core/components";
import { IFormField, IApiResult } from "@montr-core/models";
import { MetadataService } from "@montr-core/services";
import { IClassifierType } from "../models";
import { ClassifierTypeService } from "../services";
import { RouteBuilder } from "..";

interface IProps extends CompanyContextProps {
	data: IClassifierType;
	onDataChange?: (values: IClassifierType) => void
}

interface IState {
	loading: boolean;
	fields?: IFormField[];
	redirect?: string;
}

class _TabEditClassifierType extends React.Component<IProps, IState> {
	private _metadataService = new MetadataService();
	private _classifierTypeService = new ClassifierTypeService();

	constructor(props: IProps) {
		super(props);

		this.state = {
			loading: true
		};
	}

	componentDidMount = async () => {
		await this.fetchData();
	}

	componentDidUpdate = async (prevProps: IProps) => {
		if (this.props.currentCompany !== prevProps.currentCompany) {
			await this.fetchData();
		}
	}

	componentWillUnmount = async () => {
		await this._metadataService.abort();
		await this._classifierTypeService.abort();
	}

	private fetchData = async () => {
		const { currentCompany } = this.props;

		if (currentCompany) {

			const dataView = await this._metadataService.load(`ClassifierType`);

			this.setState({ loading: false, fields: dataView.fields });
		}
	}

	save = async (values: IClassifierType): Promise<IApiResult> => {

		const { data, onDataChange } = this.props;
		const { uid: companyUid } = this.props.currentCompany;

		if (data.uid) {
			const updated = { uid: data.uid, ...values };

			const result = await this._classifierTypeService.update(companyUid, updated);

			if (result.success) {
				if (onDataChange) await onDataChange(updated);
			}

			return result;
		}
		else {
			const result = await this._classifierTypeService.insert(companyUid, values);

			if (result.success) {
				this.setState({ redirect: RouteBuilder.editClassifierType(result.uid) });
			}

			return result;
		}
	}

	render() {
		const { data } = this.props,
			{ redirect, fields } = this.state;

		if (redirect) {
			this.setState({ redirect: null });
			return <Redirect to={redirect} />
		}

		return (
			<Spin spinning={this.state.loading}>
				<DataForm fields={fields} data={data} onSave={this.save} />
			</Spin>
		);
	}
}

export const TabEditClassifierType = withCompanyContext(_TabEditClassifierType);