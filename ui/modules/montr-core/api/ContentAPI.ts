import { Fetcher } from "../services";

import { Constants, IMenu } from "./";

const getMenu = async (menuId: string): Promise<IMenu> => {
	const data: IMenu = await Fetcher.post(
		`${Constants.baseURL}/Content/Menu`, { menuId: menuId });

	return data;
};

export const ContentAPI = {
	getMenu
};
