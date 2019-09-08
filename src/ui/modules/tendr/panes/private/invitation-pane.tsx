import * as React from "react";
import { Button, Icon } from "antd";
import { IPaneProps } from "@montr-core/models";
import { IEvent, IInvitation } from "../../models";
import { IPaneComponent, DataTable, Toolbar, DataTableUpdateToken } from "@montr-core/components";
import { ModalEditInvitation } from "../../components";

interface IProps extends IPaneProps<IEvent> {
	data: IEvent;
}

interface IState {
	modalData?: IInvitation;
	updateTableToken: DataTableUpdateToken;
}

export class InvitationPane extends React.Component<IProps, IState> {

	private _formRef: IPaneComponent;

	constructor(props: IProps) {
		super(props);

		this.state = {
			updateTableToken: { date: new Date() }
		};
	}

	save() {
		this._formRef.save();
	}

	refreshTable = async (resetSelectedRows?: boolean) => {
		this.setState({
			updateTableToken: { date: new Date(), resetSelectedRows }
		});
	}

	showAddModal = () => {
		this.setState({ modalData: {} });
	}

	onModalSuccess = async (data: IInvitation) => {
		this.setState({ modalData: null });

		await this.refreshTable();
	}

	onModalCancel = () => {
		this.setState({ modalData: null });
	}

	render() {
		const { modalData, updateTableToken } = this.state;

		return <>
			<ol>
				<li>Manual add</li>
				<li>Import from *.xls etc</li>
				<li>Select from registered companies</li>
				<li>Invite from companies catalogs</li>
				<li>Select from counterparty classifier</li>
				<li>Copy invitation from other event</li>
			</ol>

			<Toolbar>
				<Button onClick={this.showAddModal}><Icon type="plus" /> Добавить</Button>
			</Toolbar>

			<div style={{ clear: "both" }} />

			<DataTable
				viewId="PrivateEventCounterpartyList/Grid"
				loadUrl="/api/Company/List"
				updateToken={updateTableToken}
			/>

			{modalData &&
				<ModalEditInvitation
					onSuccess={this.onModalSuccess}
					onCancel={this.onModalCancel}
				/>}
		</>;
	}
}
