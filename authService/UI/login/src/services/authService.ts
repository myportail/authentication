class AuthService {

    public async login(username: string, password: string): Promise<any> {
        return {
            token: 'some token value',
            username
        };
    }
    
    public async logout(): Promise<any> {
        return {
            token: undefined,
            username: undefined
        };
    }
}

export default AuthService;
