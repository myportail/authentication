import {AuthenticationActionsIds} from "../actions/authenticationActionsIds";

export interface AuthenticationState {
    token: string | undefined;
    username: string | undefined;
    loginInProgress: boolean;
}

export const authenticationDefaultState : AuthenticationState = {
    token: undefined,
    username: undefined,
    loginInProgress: false
};

var authenticationReducer = (state: AuthenticationState = authenticationDefaultState, action: any) => {
    switch (action.type) {
        case AuthenticationActionsIds.loginSuccess: {
            return {
                ...state,
                token: action.token,
                username: action.username,
                loginInProgress: false,
                loginError: undefined
            };
        }
        case AuthenticationActionsIds.loginStarted: {
            return {
                ...state,
                loginInProgress: true
            }
        }
        case AuthenticationActionsIds.loginFailure: {
            return {
                ...state,
                loginInProgress: false,
                loginError: 'failure to login'
            }
        }
        case AuthenticationActionsIds.logout: {
            return {
                ...state,
                token: undefined,
                username: undefined
            };
        }
    }
    
    return state;
}


export default authenticationReducer;
