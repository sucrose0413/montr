import { Constants } from "@montr-core/.";
import { ButtonAdd, ButtonDelete, DataTable, DataTableUpdateToken, Page, PageHeader, Toolbar } from "@montr-core/components";
import { IMenu } from "@montr-core/models";
import { OperationService } from "@montr-core/services";
import { CompanyContextProps, withCompanyContext } from "@montr-kompany/components";
import * as React from "react";
import { Translation, withTranslation, WithTranslation } from "react-i18next";
import { Link } from "react-router-dom";
import { ClassifierBreadcrumb } from ".";
import { ClassifierGroup } from "../models";
import { Locale, RouteBuilder } from "../module";
import { ClassifierTypeService } from "../services";

interface Props extends CompanyContextProps, WithTranslation {
}

interface State {
	selectedRowKeys: string[] | number[];
	updateTableToken: DataTableUpdateToken;
}

class WrappedSearchClassifierType extends React.Component<Props, State> {

	private operation = new OperationService();
	private classifierTypeService = new ClassifierTypeService();

	constructor(props: Props) {
		super(props);

		this.state = {
			selectedRowKeys: [],
			updateTableToken: { date: new Date() }
		};
	}

	componentDidUpdate = async (prevProps: Props) => {
		// todo: detect company changed without CompanyContextProps (here and everywhere)
		if (this.props.currentCompany !== prevProps.currentCompany) {
			this.refreshTable(true);
		}
	};

	componentWillUnmount = async () => {
		await this.classifierTypeService.abort();
	};

	onSelectionChange = async (selectedRowKeys: string[] | number[]) => {
		this.setState({ selectedRowKeys });
	};

	delete = async () => {
		await this.operation.confirmDelete(async () => {
			const result = await this.classifierTypeService.delete(this.state.selectedRowKeys);

			if (result.success) {
				await this.refreshTable(true, true);
			}

			return result;
		});
	};

	refreshTable = async (resetCurrentPage?: boolean, resetSelectedRows?: boolean) => {
		const { selectedRowKeys } = this.state;

		this.setState({
			updateTableToken: { date: new Date(), resetCurrentPage, resetSelectedRows },
			selectedRowKeys: resetSelectedRows ? [] : selectedRowKeys
		});
	};

	render = (): React.ReactNode => {
		const { updateTableToken, selectedRowKeys } = this.state;

		const rowActions: IMenu[] = [
			{ name: "Настроить", route: (item: ClassifierGroup) => RouteBuilder.editClassifierType(item.uid) }
		];

		return (
			<Translation ns={Locale.Namespace}>
				{(t) => <Page
					title={<>
						<Toolbar float="right">
							<Link to={`/classifiers/add`}>
								<ButtonAdd type="primary" />
							</Link>
							<ButtonDelete onClick={this.delete} disabled={!selectedRowKeys?.length} />
						</Toolbar>

						<ClassifierBreadcrumb />
						<PageHeader>{t("page.searchClassifierTypes.title")}</PageHeader>
					</>}>

					<DataTable
						rowKey="uid"
						viewId={`ClassifierType/Grid/`}
						loadUrl={`${Constants.apiURL}/classifierType/list/`}
						onSelectionChange={this.onSelectionChange}
						updateToken={updateTableToken}
						rowActions={rowActions}
					/>

				</Page>}
			</Translation>
		);
	};
}

const SearchClassifierType = withTranslation()(withCompanyContext(WrappedSearchClassifierType));

export default SearchClassifierType;
