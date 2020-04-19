import React from 'react';
import './App.scss';
import ConfigService from "./services/configService";
import LoginPage from "./components/loginPage/loginPage";

class App extends React.Component {
  public constructor(props: {}) {
    super(props);
  }
  
  public render() {
    return (
        <div className="App">
            <LoginPage></LoginPage>
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
