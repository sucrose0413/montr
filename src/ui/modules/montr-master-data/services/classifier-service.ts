import { Fetcher } from "@montr-core/services";
import { Constants } from "@montr-core/.";
import { Guid, IApiResult } from "@montr-core/models";
import { IClassifier } from "../models";

interface IInsertClassifierRequest {
	typeCode: string;
	item: IClassifier;
}

export class ClassifierService extends Fetcher {

	get = async (companyUid: Guid, typeCode: string, uid: Guid | string): Promise<IClassifier> => {
		return this.post(`${Constants.baseURL}/classifier/get`, { companyUid, typeCode, uid });
	};

	export = async (companyUid: Guid, request: any): Promise<IClassifier> => {
		return this.download(`${Constants.baseURL}/classifier/export`, { companyUid, ...request });
	};

	insert = async (companyUid: Guid, request: IInsertClassifierRequest): Promise<IApiResult> => {
		return this.post(`${Constants.baseURL}/classifier/insert`, { companyUid, ...request });
	};

	update = async (companyUid: Guid, typeCode: string, data: IClassifier): Promise<IApiResult> => {
		return this.post(`${Constants.baseURL}/classifier/update`, { companyUid, typeCode, item: data });
	};

	delete = async (companyUid: Guid, typeCode: string, uids: string[] | number[]): Promise<number> => {
		return this.post(`${Constants.baseURL}/classifier/delete`, { companyUid, typeCode, uids });
	};
}