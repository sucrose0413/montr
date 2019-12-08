import React from "react";
import { Modal, Spin } from "antd";
import { Translation } from "react-i18next";
import { IFormField, IApiResult } from "@montr-core/models";
import { DataForm, WrappedDataForm } from "@montr-core/components";
import { MetadataService } from "@montr-core/services";
import { ProfileService } from "../services";
import { IProfileModel, IChangePasswordModel } from "../models";
import { Views } from "../module";

interface IProps {
	onSuccess?: () => void;
	onCancel?: () => void;
}

interface IState {
	loading: boolean;
	data: IProfileModel;
	fields?: IFormField[];
}

export class ModalChangePassword extends React.Component<IProps, IState> {

	private _metadataService = new MetadataService();
	private _profileService = new ProfileService();

	_formRef: WrappedDataForm;

	constructor(props: IProps) {
		super(props);

		this.state = {
			loading: true,
			data: {}
		};
	}

	componentDidMount = async () => {
		await this.fetchData();
	};

	componentWillUnmount = async () => {
		await this._metadataService.abort();
		await this._profileService.abort();
	};

	fetchData = async () => {
		const data = await this._profileService.get();

		const dataView = await this._metadataService.load(Views.formChangePassword);

		this.setState({ loading: false, data, fields: dataView.fields });
	};

	saveFormRef = (formRef: WrappedDataForm) => {
		this._formRef = formRef;
	};

	onOk = async (e: React.MouseEvent<any>) => {
		await this._formRef.handleSubmit(e);
	};

	onCancel = () => {
		if (this.props.onCancel) this.props.onCancel();
	};

	handleSubmit = async (values: IChangePasswordModel): Promise<IApiResult> => {
		const { onSuccess } = this.props;

		const result = await this._profileService.changePassword(values);

		if (result.success) {
			if (onSuccess) await onSuccess();
		}

		return result;
	};

	render = () => {
		const { loading, fields, data } = this.state;

		return (
			<Translation ns="idx">
				{(t) => <Modal visible={!loading} title={t("page.changePassword.title")}
					onOk={this.onOk} onCancel={this.onCancel}
					okText="Update password" /* width="640px" */>

					<p>A strong password helps prevent unauthorized access to your account.</p>

					<Spin spinning={loading}>
						<DataForm
							wrappedComponentRef={this.saveFormRef}
							fields={fields}
							data={data}
							showControls={false}
							onSubmit={this.handleSubmit}
						/>
					</Spin>
				</Modal>}
			</Translation>
		);
	};
}