import * as React from "react";
import { useTranslation } from "react-i18next";
import { Button, Spin, Icon, Divider } from "antd";
import { Toolbar } from "./toolbar";
import { IDataField } from "@montr-core/models";
import { DataTable, DataTableUpdateToken } from ".";
import { Constants } from "..";

interface IProps {
	entityTypeCode: string;
}

interface IState {
	loading: boolean;
	fields?: IDataField[];
	updateTableToken?: DataTableUpdateToken;
}

export function PaneEditMetadata(props: IProps) {

	const { t } = useTranslation(),
		[state, setState] = React.useState<IState>({ loading: true, updateTableToken: { date: new Date() } });

	React.useEffect(() => {
		async function fetchData() {
			const fields: IDataField[] = [
				{ key: "fullName", name: "Полное наименование", type: "string" },
				{ key: "shortName", name: "Сокращенное наименование", type: "string" },
				{ key: "address", name: "Адрес в пределах места пребывания", type: "address" },
				{ key: "okved", name: "Код(ы) ОКВЭД", type: "classifier" },
				{ key: "inn", name: "ИНН", type: "string" },
				{ key: "kpp", name: "КПП", type: "string" },
				{ key: "dp", name: "Дата постановки на учет в налоговом органе", type: "date" },
				{ key: "ogrn", name: "ОГРН", type: "string" },
				{ key: "is_msp", name: "Участник закупки является субъектом малого предпринимательства", type: "boolean" },
			];

			setState({ loading: false, fields: fields });
		}

		fetchData();

		return async () => {
			// await metadataService.abort();
		};
	}, []);

	const { entityTypeCode } = props,
		{ loading, fields, updateTableToken } = state;

	return (
		<Spin spinning={loading}>
			<Toolbar>
				<Button><Icon type="plus" /> {t("button.add")}</Button>
			</Toolbar>

			<Divider />

			<DataTable
				rowKey="key"
				// rowActions={rowActions}
				viewId={`Metadata/Grid`}
				loadUrl={`${Constants.apiURL}/metadata/list/`}
				updateToken={updateTableToken}
			/>
		</Spin>
	);
}
