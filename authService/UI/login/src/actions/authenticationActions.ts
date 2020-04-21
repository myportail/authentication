import {ActionsBase} from './actionsBase';

interface AuthenticationActionsIds {
    login: string;
    logout: string;
}

class AuthenticationActions extends ActionsBase<AuthenticationActionsIds> {

    constructor() {
        super([
            'login',
            'logout'
        ]);
    }
    
    public login = (username: string, password: string) => {
        return {
            type: this.ids.login,
            username,
            password
        };
    }
    
    public logout = () => {
        return {
            type: this.ids.logout
        };
    }
}

const authenticationActionsInstance = new AuthenticationActions();

export default authenticationActionsInstance;
