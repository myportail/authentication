import React from "react";
import './header.scss'
import UserInfo from "./userInfo/userInfo";
import {connect} from "react-redux";

interface HeaderProps {
    authentication: any;
}

let Header = (props: HeaderProps) => (
    <div className="header-component">
        <div className="user-info">
            { (!!props.authentication.token) ? <UserInfo /> : '' }
        </div>
    </div>
);

const mapStateToProps = (state: any) => {
    return {
        authentication: state.authentication
    }
};


export default connect(mapStateToProps)(Header);
