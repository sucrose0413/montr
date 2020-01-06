import React from "react";
import { AppRouteRegistry } from "./services";
import { Layout } from "./constants";

import "./i18n";
import "./index.less";

AppRouteRegistry.add([
	{ path: "/", layout: Layout.public, exact: true, component: React.lazy(() => import("./components/page-home")) },

	{ path: "/dashboard/", exact: true, component: React.lazy(() => import("./components/page-dashboard")) },
	{ path: "/locales/", exact: true, component: React.lazy(() => import("./components/page-search-locale-string")) },
	{ path: "/settings/", exact: true, component: React.lazy(() => import("./components/page-settings")) },
]);
