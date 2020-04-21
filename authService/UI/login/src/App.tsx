import React from 'react';
import './App.scss';
import ConfigService, {IConfigService} from "./services/configService";
import LoginPage from "./components/loginPage/loginPage";
import {connect} from "react-redux";
import MainPage from "./components/mainPage/mainPage";

interface AppProps {
    isAuthenticated: boolean;
}

class App extends React.Component<AppProps> {
    public constructor(props: AppProps) {
        super(props);
    }

    public render() {
        return (
            <div className="App">
                {this.props.isAuthenticated ? <MainPage/> : <LoginPage/>}
            </div>
        );
    }

    public componentDidMount(): void {
        console.log(process.env);
        var service = ConfigService.instance;
        service.load()
            .then((config) => {
                console.log('config loaded');
            });
    }
}

const mapStateToProps = (state: any) => {
    return {
        isAuthenticated: !!state.authentication.token,
        authentication: state.authentication
    }
};

export default connect(mapStateToProps)(App);
