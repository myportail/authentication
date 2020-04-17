import axios from "axios";

export default class ConfigService {
    
    public static get instance(): ConfigService {
        if (!ConfigService.instance_) {
            ConfigService.instance_ = new ConfigService();
        }
        
        return ConfigService.instance_;
    }
    
    public async load() {
        const config = await axios.get('/config.json', {
            responseType: 'json'
        });
        return config.data;
    }

    private static instance_: ConfigService;

    private constructor() {

    }
}
