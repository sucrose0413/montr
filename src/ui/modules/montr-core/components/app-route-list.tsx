import React from "react"
import { Switch } from "react-router"
import { BrowserRouter } from "react-router-dom"
import { AppRoute } from "./"
import { IRoute } from "../models"
import { Error404 } from "../pages"

interface IProps {
	routes: IRoute[];
	layoutRegistry: (key: string) => React.ComponentType<any>;
	defaultLayout: string;
	errorLayout: string;
}

export const AppRouteList = ({ routes, layoutRegistry, defaultLayout, errorLayout }: IProps) => (
	<BrowserRouter>
		<Switch>
			{routes.map(({ layout, ...props }, _do_not_use_) => {
				return <AppRoute key={0} {...props} layoutComponent={layoutRegistry(layout || defaultLayout)} />
			})}
			<AppRoute component={Error404} layoutComponent={layoutRegistry(errorLayout)} />
		</Switch>
	</BrowserRouter>
)