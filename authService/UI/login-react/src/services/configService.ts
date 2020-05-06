import axios from "axios";

export interface IConfigService {
    load(): Promise<any>;
}

export default class ConfigService implements IConfigService {
    
    public static get instance(): ConfigService {
        if (!ConfigService.instance_) {
            ConfigService.instance_ = new ConfigService();
        }
        
        return ConfigService.instance_;
    }
    
    public async load() : Promise<any> {
        const config = await axios.get('/config.json', {
            responseType: 'json'
        });
        return config.data;
    }

    private static instance_: ConfigService;

    private constructor() {

    }
}
