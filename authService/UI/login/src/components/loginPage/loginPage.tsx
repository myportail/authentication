import React from "react";
import Login from "../login/login";
import "./loginPage.scss"

class LoginPage extends React.Component {
    public render() {
        return (
            <div className="login-page">
                <div className="login-controls">
                    <Login></Login>
                </div>
            </div>
        );
    }
}

export default LoginPage;
