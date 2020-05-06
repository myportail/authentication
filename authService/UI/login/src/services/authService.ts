interface IAuthService {
    
    login(username: string, password: string): Promise<any>;
    logout(): Promise<any>;
}

class AuthService implements IAuthService {

    public async login(username: string, password: string): Promise<any> {
        return new Promise((resolve) => {
            setTimeout(() => {
                resolve({
                    token: 'some token value',
                    username
                });
            }, 3000)
        });
    }
    
    public async logout(): Promise<any> {
        return {
            token: undefined,
            username: undefined
        };
    }
}

let authService: IAuthService = new AuthService();

export default authService;
