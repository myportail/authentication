import React from "react";
import './login.scss';

class Login extends React.Component {
    onSubmit = (e: any) => {
        e.preventDefault();
        const username = e.target.username.value;
        const password = e.target.password.value;
        console.log('test');
    };
    public render() {
        return (
            <div>
                <form onSubmit={this.onSubmit} className="login-form">
                    <div className="controls-line">
                        <label htmlFor="username">username: </label>
                        <input id="username"/>
                    </div>
                    <div className="controls-line">
                        <label htmlFor="password">password: </label>
                        <input id="password" type="password"/>
                    </div>
                    <div className="controls-line">
                        <div></div>
                        <input type="submit" value="login" />
                    </div>
                </form>
            </div>
        );
    }
};

export default Login;
