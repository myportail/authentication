class AuthenticationActions {

    public static ids = {
        login: 'AUTHENTICATION_LOGIN',
        logout: 'AUTHENTICATION_LOGOUT'
    };
    
    public static login = (username: string, password: string) => {
        return {
            type: AuthenticationActions.ids.login,
            username,
            password
        };
    }
    
    public static logout = () => {
        return {
            type: AuthenticationActions.ids.logout
        };
    }
}

export default AuthenticationActions;
