import React from "react";
import {connect} from "react-redux";
import AuthenticationActions from "../../actions/authentication";

interface MainPageProps {
    dispatch(args: any): any;
}

class MainPage extends React.Component<MainPageProps> {
    logout = (e: any) => {
        e.preventDefault();
        this.props.dispatch(AuthenticationActions.logout());
    }
    
    render() {
        return (
            <button onClick={this.logout}>logout</button>
        )
    }
}

export default connect()(MainPage);
