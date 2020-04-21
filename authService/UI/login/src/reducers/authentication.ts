import AuthenticationActions from "../actions/authenticationActions";

const authenticationDefaultState = {
    token: undefined,
    username: undefined
};

var reducer = (state = authenticationDefaultState, action: any) => {
    
    switch (action.type) {
        case AuthenticationActions.ids.login: {
            return {
                ...state,
                token: 'some value',
                username: 'user name'
            };
        }
        break;
        case AuthenticationActions.ids.logout: {
            return {
                ...state,
                token: undefined,
                username: undefined
            };
        }
        break;
    }
    
    return state;
}


export default reducer;
