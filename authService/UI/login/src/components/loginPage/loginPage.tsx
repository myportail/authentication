import React from "react";
import Login from "../login/login";
import "./loginPage.scss"
import {connect} from "react-redux";

class LoginPage extends React.Component {
    public render() {
        return (
            <div className="login-page">
                <div className="login-controls">
                    <div className="login-controls-row">
                        <Login></Login>
                    </div>
                </div>
            </div>
        );
    }
}


export default connect()(LoginPage);
