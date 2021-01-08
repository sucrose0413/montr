import * as React from "react";
import { Spin } from "antd";
import { Translation } from "react-i18next";
import { ApiResult, IDataField, IIndexer } from "../models";
import { MetadataService, SetupService } from "../services";
import { DataForm, Page } from ".";
import { Views } from "../module";

interface Props {
}

interface State {
    loading: boolean;
    fields?: IDataField[];
}

export default class PageSetup extends React.Component<Props, State> {

    private _metadataService = new MetadataService();
    private _setupService = new SetupService();

    constructor(props: Props) {
        super(props);

        this.state = {
            loading: true
        };
    }

    componentDidMount = async () => {
        await this.fetchData();
    };

    componentWillUnmount = async () => {
        await this._metadataService.abort();
        await this._setupService.abort();
    };

    fetchData = async () => {
        const dataView = await this._metadataService.load(Views.setupForm);

        this.setState({ loading: false, fields: dataView.fields });
    };

    // todo: redirect to login after success
    save = async (values: IIndexer): Promise<ApiResult> => {
        return await this._setupService.save(values);
    };

    render = () => {
        const { loading, fields } = this.state, data = {};

        return (
            <Translation>
                {(t) => <Page title={t("page.setup.title")}>

                    <p>{t("page.setup.subtitle")}</p>

                    <Spin spinning={loading}>
                        <DataForm
                            layout="vertical"
                            fields={fields}
                            data={data}
                            onSubmit={this.save}
                        />
                    </Spin>

                </Page>}
            </Translation>
        );
    };
}