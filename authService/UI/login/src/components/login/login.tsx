import React from "react";

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
                    <label htmlFor="username">username: </label>
                    <input id="username"/>
                    <label htmlFor="password">password: </label>
                    <input id="password" type="password"/>
                    <input type="submit" value="login" />
                </form>
            </div>
        );
    }
};

export default Login;
