import * as React from "react";
import { Route } from "react-router";
import { Dashboard, SearchEvents, SelectEventTemplate, EditEvent } from ".";
import { SearchClassifier, EditClassifier } from "@montr-master-data/pages";

export const Routes = () => {
	return <>
		<Route path="/" exact component={() => <Dashboard />} />
		<Route path="/events" exact component={() => <SearchEvents />} />
		<Route path="/events/new" component={() => <SelectEventTemplate />} />
		<Route path="/events/edit/:id" component={({ match }: any) => <EditEvent {...match} />} />
		<Route path="/classifiers/:configCode" exact component={SearchClassifier} />
		<Route path="/classifiers/:configCode/new" exact component={EditClassifier} />
	</>
}
