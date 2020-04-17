import React from 'react';
import logo from './logo.svg';
import './App.css';
import {Helmet} from "react-helmet";
import ConfigService from "./services/configService";

class App extends React.Component {
  public constructor(props: {}) {
    super(props);
  }
  
  public render() {
    return (
        <div className="App">
          <header className="App-header">
            <img src={logo} className="App-logo" alt="logo" />
            <p>
              Edit <code>src/App.tsx</code> and save to reload.
            </p>
            <a
                className="App-link"
                href="https://reactjs.org"
                target="_blank"
                rel="noopener noreferrer"
            >
              Learn React
            </a>
          </header>
        </div>
    );
  }
  
  public componentDidMount(): void {
    var service = ConfigService.instance;
    service.load()
        .then((config) => {
          console.log('config loaded');
        });
  }
}

export default App;
