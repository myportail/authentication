import {ActionsBase} from './actionsBase';

interface AuthenticationActionsIds {
    loginStarted: string;
    loginSuccess: string;
    loginFailure: string;
    logout: string;
}

class AuthenticationActions extends ActionsBase<AuthenticationActionsIds> {

    constructor() {
        super([
            'loginStarted',
            'loginSuccess',
            'loginFailure',
            'logout'
        ]);
    }
    
    public login = (username: string, password: string) => {
        return (dispatch: any) => {
            dispatch(this.loginStarted());
            
            setTimeout(() => {
                dispatch(this.loginSuccess('some token value', 'some username'));
            }, 5000);
        };
    }
    
    public loginStarted = () => {
        return {
            type: this.ids.loginStarted
        };
    }
    
    public loginSuccess = (token: string, username: string) => {
        return {
            type: this.ids.loginSuccess,
            token,
            username
        };
    }
    
    public loginFailure = (error: string) => {
        return {
            type: this.ids.loginFailure,
            error
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
