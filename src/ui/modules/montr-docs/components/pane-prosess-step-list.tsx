import React from "react";

interface Props {
}

interface State {
    loading: boolean;
}

export default class PaneProcessStepList extends React.Component<Props, State> {

    render = (): React.ReactNode => {
        return <h1>PaneProcessStepList</h1>;
    };
}
