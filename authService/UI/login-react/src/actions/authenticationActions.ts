import { AuthenticationActionsIds } from './authenticationActionsIds'

class AuthenticationActions {
    
    public static login = (username: string, password: string) => {
        return (dispatch: any) => {
            dispatch(AuthenticationActions.loginStarted());
            
            setTimeout(() => {
                dispatch(AuthenticationActions.loginSuccess('some token value', 'some username'));
            }, 5000);
        };
    }
    
    public static loginStarted = () => {
        return {
            type: AuthenticationActionsIds.loginStarted
        };
    }
    
    public static loginSuccess = (token: string, username: string) => {
        return {
            type: AuthenticationActionsIds.loginSuccess,
            token,
            username
        };
    }
    
    public static loginFailure = (error: string) => {
        return {
            type: AuthenticationActionsIds.loginFailure,
            error
        };
    }
    
    public static logout = () => {
        return {
            type: AuthenticationActionsIds.logout
        };
    }
}

export default AuthenticationActions;
