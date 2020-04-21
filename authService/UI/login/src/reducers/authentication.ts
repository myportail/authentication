import AuthenticationActions from "../actions/authenticationActions";

const authenticationDefaultState = {
    token: undefined,
    username: undefined
};

var reducer = (state = authenticationDefaultState, action: any) => {
    
    switch (action.type) {
        case AuthenticationActions.ids.loginSuccess: {
            return {
                ...state,
                token: action.token,
                username: action.username,
                loginInProgress: false,
                loginError: undefined
            };
        }
        case AuthenticationActions.ids.loginStarted: {
            return {
                ...state,
                loginInProgress: true
            }
        }
        case AuthenticationActions.ids.loginFailure: {
            return {
                ...state,
                loginInProgress: false,
                loginError: 'failure to login'
            }
        }
        case AuthenticationActions.ids.logout: {
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
