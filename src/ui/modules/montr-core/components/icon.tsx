import * as React from "react";
import ArrowDownOutlined from "@ant-design/icons/ArrowDownOutlined";
import ArrowLeftOutlined from "@ant-design/icons/ArrowLeftOutlined";
import ArrowUpOutlined from "@ant-design/icons/ArrowUpOutlined";
import BarChartOutlined from "@ant-design/icons/BarChartOutlined";
import CheckOutlined from "@ant-design/icons/CheckOutlined";
import CheckCircleOutlined from "@ant-design/icons/CheckCircleOutlined";
import CheckCircleTwoTone from "@ant-design/icons/CheckCircleTwoTone";
import ClusterOutlined from "@ant-design/icons/ClusterOutlined";
import ContainerOutlined from "@ant-design/icons/ContainerOutlined";
import DashboardOutlined from "@ant-design/icons/DashboardOutlined";
import DeleteOutlined from "@ant-design/icons/DeleteOutlined";
import DownOutlined from "@ant-design/icons/DownOutlined";
import EditOutlined from "@ant-design/icons/EditOutlined";
import ExportOutlined from "@ant-design/icons/ExportOutlined";
import FacebookOutlined from "@ant-design/icons/FacebookOutlined";
import FileOutlined from "@ant-design/icons/FileOutlined";
import FolderOutlined from "@ant-design/icons/FolderOutlined";
import GoogleOutlined from "@ant-design/icons/GoogleOutlined";
import HomeOutlined from "@ant-design/icons/HomeOutlined";
import Html5Outlined from "@ant-design/icons/Html5Outlined";
import Html5TwoTone from "@ant-design/icons/Html5TwoTone";
import ImportOutlined from "@ant-design/icons/ImportOutlined";
import GlobalOutlined from "@ant-design/icons/GlobalOutlined";
import LeftOutlined from "@ant-design/icons/LeftOutlined";
import LockOutlined from "@ant-design/icons/LockOutlined";
import LogoutOutlined from "@ant-design/icons/LogoutOutlined";
import MinusCircleOutlined from "@ant-design/icons/MinusCircleOutlined";
import PlusOutlined from "@ant-design/icons/PlusOutlined";
import ProjectOutlined from "@ant-design/icons/ProjectOutlined";
import QuestionCircleOutlined from "@ant-design/icons/QuestionCircleOutlined";
import SearchOutlined from "@ant-design/icons/SearchOutlined";
import SelectOutlined from "@ant-design/icons/SelectOutlined";
import SettingOutlined from "@ant-design/icons/SettingOutlined";
import UserOutlined from "@ant-design/icons/UserOutlined";
import WindowsOutlined from "@ant-design/icons/WindowsOutlined";

export abstract class Icon {
	private static Map: { [key: string]: JSX.Element; } = {};

	static get(key: string): JSX.Element {
		return Icon.Map[key];
	}

	static ArrowDown = Icon.Map["arrow-down"] = <ArrowDownOutlined />;
	static ArrowLeft = Icon.Map["arrow-left"] = <ArrowLeftOutlined />;
	static ArrowUp = Icon.Map["arrow-up"] = <ArrowUpOutlined />;
	static BarChart = Icon.Map["bar-chart"] = <BarChartOutlined />;
	static Check = Icon.Map["check"] = <CheckOutlined />;
	static CheckCircle = Icon.Map["check-circle"] = <CheckCircleOutlined />;
	static CheckCircleTwoTone = Icon.Map["check-circle-2t"] = <CheckCircleTwoTone twoToneColor="#52c41a" />;
	static Cluster = Icon.Map["cluster"] = <ClusterOutlined />;
	static Container = Icon.Map["container"] = <ContainerOutlined />;
	static Dashboard = Icon.Map["dashboard"] = <DashboardOutlined />;
	static Delete = Icon.Map["delete"] = <DeleteOutlined />;
	static Down = Icon.Map["down"] = <DownOutlined />;
	static Edit = Icon.Map["edit"] = <EditOutlined />;
	static Export = Icon.Map["export"] = <ExportOutlined />;
	static Facebook = Icon.Map["facebook"] = <FacebookOutlined />;
	static File = Icon.Map["file"] = <FileOutlined />;
	static Folder = Icon.Map["folder"] = <FolderOutlined />;
	static Google = Icon.Map["google"] = <GoogleOutlined />;
	static Home = Icon.Map["home"] = <HomeOutlined />;
	static Html5 = Icon.Map["html5"] = <Html5Outlined />;
	static Html5TwoTone = Icon.Map["html5-2t"] = <Html5TwoTone />;
	static Import = Icon.Map["import"] = <ImportOutlined />;
	static Global = Icon.Map["global"] = <GlobalOutlined />;
	static Left = Icon.Map["left"] = <LeftOutlined />;
	static Lock = Icon.Map["lock"] = <LockOutlined />;
	static Logout = Icon.Map["login"] = <LogoutOutlined />;
	static MinusCircle = Icon.Map["minus-circle"] = <MinusCircleOutlined />;
	static Plus = Icon.Map["plus"] = <PlusOutlined />;
	static Project = Icon.Map["project"] = <ProjectOutlined />;
	static QuestionCircle = Icon.Map["question-circle"] = <QuestionCircleOutlined />;
	static Search = Icon.Map["search"] = <SearchOutlined />;
	static Select = Icon.Map["select"] = <SelectOutlined />;
	static Setting = Icon.Map["setting"] = <SettingOutlined />;
	static User = Icon.Map["user"] = <UserOutlined />;
	static Windows = Icon.Map["windows"] = <WindowsOutlined />;
}
