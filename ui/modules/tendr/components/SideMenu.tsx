import * as React from "react";
import { Link } from "react-router-dom";
import { Menu, Icon } from "antd";
import { UserWithCompanyMenu } from "@kompany/components";

export class SideMenu extends React.Component {

	// todo: load menu items from server
	render() {
		return (
			<Menu theme="dark" mode="vertical">
				<Menu.Item key="1">
					<Link to="/">
						<Icon type="dashboard" />
						<span className="nav-text">Панель управления</span>
					</Link>
				</Menu.Item>
				<Menu.Item key="2">
					<Link to="/events">
						<Icon type="project" />
						<span className="nav-text">Торговые процедуры</span>
					</Link>
				</Menu.Item>
				<Menu.Item key="999">
					<a href="http://tendr.montr.io:5000/">
						<Icon type="global" />
						<span className="nav-text">Public</span>
					</a>
				</Menu.Item>

				<UserWithCompanyMenu />
			</Menu>
		);
	}
};
