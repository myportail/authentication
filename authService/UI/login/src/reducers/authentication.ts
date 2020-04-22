import {AuthenticationActionsIds} from "../actions/authenticationActionsIds";

const authenticationDefaultState = {
    token: undefined,
    username: undefined
};

var reducer = (state = authenticationDefaultState, action: any) => {
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


export default reducer;
