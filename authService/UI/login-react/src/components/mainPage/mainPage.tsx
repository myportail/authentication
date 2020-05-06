import React from "react";
import {connect} from "react-redux";
import AuthenticationActions from "../../actions/authenticationActions";

interface MainPageProps {
    dispatch(args: any): any;
}

class MainPage extends React.Component<MainPageProps> {
    render() {
        return (
            <div/>
        )
    }
}

export default connect()(MainPage);
