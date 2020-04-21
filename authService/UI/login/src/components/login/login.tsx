import React from "react";
import './login.scss';
import AuthenticationActions from "../../actions/authenticationActions";
import {connect} from "react-redux";

interface LoginProps {
    dispatch(args: any): any;
}

class Login extends React.Component<LoginProps> {
    state = {
        username: '',
        password: ''
    };
    onUsernameChange = (e: any) => {
        e.preventDefault();
        this.setState({
            username: e.target.value
        });
    };
    onPasswordChange = (e: any) => {
        e.preventDefault();
        this.setState({
            password: e.target.value
        });
    };
    onSubmit = (e: any) => {
        e.preventDefault();
        this.props.dispatch(AuthenticationActions.login(
            this.state.username,
            this.state.password
        ));
    };
    public render() {
        return (
            <div>
                <form onSubmit={this.onSubmit} className="login-form">
                    <div className="controls-line">
                        <label htmlFor="username">username: </label>
                        <input id="username" value={this.state.username} onChange={this.onUsernameChange}/>
                    </div>
                    <div className="controls-line">
                        <label htmlFor="password">password: </label>
                        <input id="password" type="password" value={this.state.password} onChange={this.onPasswordChange}/>
                    </div>
                    <div className="controls-line">
                        <div/>
                        <input type="submit" value="login" />
                    </div>
                </form>
            </div>
        );
    }
}

const mapStateToProps = (state: any) => {
    return {
        authentication: state.authentication
    }
};

export default connect(mapStateToProps)(Login);
