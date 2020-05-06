import {connect} from "react-redux";
import React from "react";
import './userInfo.scss'
import AuthenticationActions from "../../../actions/authenticationActions";

interface UserInfoProps {
    authentication: any;
    dispatch: any;
}

class UserInfo extends React.Component<UserInfoProps> {
    
    logout = (e: any) => {
        e.preventDefault();
        this.props.dispatch(AuthenticationActions.logout());
    }
    
    render() {
        return (
            <div className="user-info-component">
                <span className="username">{this.props.authentication.username}</span>
                <button onClick={this.logout}>logout</button>
            </div>
        )
    }
}

const mapStateToProps = (state: any) => {
    return {
        authentication: state.authentication
    }
};

export default connect(mapStateToProps)(UserInfo);
