import * as React from "react";
import { Page, DataForm } from "../components";

export class Login extends React.Component {
	render() {
		return (
			<Page title="Log in">

				<p>Use a local account to log in.</p>

				{/* <DataForm>

				</DataForm> */}

				<p>Use another service to log in.</p>

			</Page>
		);
	}
}
