import * as React from "react";
import { Page, DataForm } from "@montr-core/components";
import { Spin, Button } from "antd";
import { RouteComponentProps } from "react-router-dom";
import { Translation } from "react-i18next";
import { AccountService } from "../services/account-service";
import { Patterns } from "../routes";
import { MetadataService } from "@montr-core/services";
import { IResetPasswordModel } from "../models";
import { IApiResult, IFormField } from "@montr-core/models";

interface IRouteProps {
	code: string;
}

interface IProps extends RouteComponentProps<IRouteProps> {
}

interface IState {
	loading: boolean;
	fields?: IFormField[];
}

export default class ResetPassword extends React.Component<IProps, IState> {

	private _metadataService = new MetadataService();
	private _accountService = new AccountService();

	constructor(props: IProps) {
		super(props);

		this.state = {
			loading: true
		};
	}

	componentDidMount = async () => {
		await this.fetchData();
	}

	componentWillUnmount = async () => {
		await this._metadataService.abort();
		await this._accountService.abort();
	}

	fetchData = async () => {
		const dataView = await this._metadataService.load("ResetPassword/Form");

		this.setState({ loading: false, fields: dataView.fields });
	}

	resetPassword = async (values: IResetPasswordModel): Promise<IApiResult> => {
		const { code } = this.props.match.params;

		return await this._accountService.resetPassword({ code, ...values });
	}

	handleContinue = async () => {
		this.props.history.push(Patterns.login);
	}

	render = () => {
		const { fields, loading } = this.state;

		return (
			<Translation ns="idx">
				{(t) => <Page title={t("page.resetPassword.title")}>

					<h3>{t("page.resetPassword.subtitle")}</h3>

					<div style={{ width: "50%" }} >
						<Spin spinning={loading}>
							<DataForm
								fields={fields}
								data={{}}
								onSubmit={this.resetPassword}
								submitButton={t("button.resetPassword")}
							/>
						</Spin>
					</div>

					<Spin spinning={loading}>

						{!loading && <p>Your password has been reset.</p>}

						<Button disabled={loading} onClick={this.handleContinue}>{t("button.continue")}</Button>
					</Spin>
				</Page>}
			</Translation>
		);
	}
}